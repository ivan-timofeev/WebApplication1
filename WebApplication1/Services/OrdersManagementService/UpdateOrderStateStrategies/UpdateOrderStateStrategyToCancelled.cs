using System.Data;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Models;
using WebApplication1.Abstractions.Services;

namespace WebApplication1.Services.OrdersManagementService.UpdateOrderStateStrategies;

class UpdateOrderStateStrategyToCancelled
    : IUpdateOrderStateStrategy
{
    private readonly ISaleItemsRepository _saleItemsRepository;
    private readonly IDatabaseTransactionsManagementService _databaseTransactionsManagementService;

    public UpdateOrderStateStrategyToCancelled(
        ISaleItemsRepository saleItemsRepository,
        IDatabaseTransactionsManagementService databaseTransactionsManagementService)
    {
        _saleItemsRepository = saleItemsRepository;
        _databaseTransactionsManagementService = databaseTransactionsManagementService;
    }
    
    public int Priority => 1;
    public OrderStateEnum[] FromStates => new[]
    {
        OrderStateEnum.Created,
        OrderStateEnum.AwaitingPayment,
        OrderStateEnum.Assembling,
        OrderStateEnum.AwaitingCustomerPickup,
        OrderStateEnum.AwaitingForDelivery,
        OrderStateEnum.DeliveryInProgress
    };
    public OrderStateEnum ToState => OrderStateEnum.Canceled;

    public void UpdateOrder(Order order)
    {
        // Remove products reservation

        _databaseTransactionsManagementService.ExecuteInTransaction(IsolationLevel.Serializable, action: () =>
        {
            var saleItemIds = order.OrderedItems
                .Select(x => x.SaleItemId)
                .ToArray();
            var saleItems = _saleItemsRepository
                .Read(saleItemIds)
                .ToArray();

            foreach (var orderItem in order.OrderedItems)
            {
                var saleItem = saleItems.First(x => x.Id == orderItem.SaleItemId);
                saleItem.Quantity += orderItem.Quantity;

                _saleItemsRepository.Update(saleItem.Id, saleItem);
            }

            OrderStateUtils.AddOrderState(order, OrderStateEnum.Canceled, 
                enterDescription: "Items unreserved. Order cancelled.");
        });
    }
}
