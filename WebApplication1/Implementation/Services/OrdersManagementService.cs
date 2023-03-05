using System.Data;
using AutoMapper;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Abstraction.Services;
using WebApplication1.Common.Exceptions;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.SearchEngine;

namespace WebApplication1.Services;

public class OrdersManagementService : IOrdersManagementService
{
    private readonly IMapper _mapper;
    private readonly IOrdersRepository _ordersRepository;
    private readonly ICustomersRepository _customersRepository;
    private readonly ISalePointsRepository _salePointsRepository;
    private readonly ISaleItemsRepository _saleItemsRepository;
    private readonly IShoppingCartsManagementService _shoppingCartsManagementService;
    private readonly IDatabaseTransactionsManagementService _databaseTransactionsManagementService;

    public OrdersManagementService(
        IMapper mapper,
        IOrdersRepository ordersRepository,
        ICustomersRepository customersRepository,
        ISalePointsRepository salePointsRepository,
        ISaleItemsRepository saleItemsRepository,
        IShoppingCartsManagementService shoppingCartsManagementService,
        IDatabaseTransactionsManagementService databaseTransactionsManagementService)
    {
        _mapper = mapper;
        _ordersRepository = ordersRepository;
        _customersRepository = customersRepository;
        _salePointsRepository = salePointsRepository;
        _saleItemsRepository = saleItemsRepository;
        _shoppingCartsManagementService = shoppingCartsManagementService;
        _databaseTransactionsManagementService = databaseTransactionsManagementService;
    }

    public Guid CreateOrder(Guid customerId)
    {
        var shoppingCart = _shoppingCartsManagementService.GetShoppingCartByCustomer(customerId);
        var customer = _customersRepository.Read(shoppingCart.CustomerId);
        
        var isTransactionCommitted = false;
        _databaseTransactionsManagementService.BeginTransaction(IsolationLevel.Serializable);
        
        var order = new Order
        {
            OrderStateHierarchical = new List<OrderStateHierarchicalItem>()
            {
                new ()
                {
                    SerialNumber = 1,
                    EnteredDateTimeUtc = DateTime.UtcNow,
                    State = OrderStateEnum.Creating,
                    EnterDescription = "Заказ создан"
                }
            },
            CustomerId = customer.Id,
            Customer = customer
        };

        try
        {
            var saleItemIds = shoppingCart.CartItems
                .Select(x => x.SaleItemId)
                .ToArray();

            var saleItems = _saleItemsRepository
                .Read(saleItemIds)
                .ToArray();

            foreach (var cartItem in shoppingCart.CartItems)
            {
                var saleItem = saleItems.First(x => x.Id == cartItem.SaleItemId);

                if (saleItem.Quantity < cartItem.Quantity)
                {
                    var errorVm = new ErrorVmBuilder()
                        .WithGlobalError("There is not enough product in stock.")
                        .WithInfo("SalePointId", saleItem.SalePointId)
                        .WithInfo("SaleItemId", saleItem.Id)
                        .WithInfo("RequiredQuantity", cartItem.Quantity)
                        .WithInfo("AvailableQuantity", saleItem.Quantity)
                        .Build();

                    throw new BusinessErrorException("There is not enough product in stock.",
                        StatusCodes.Status409Conflict, errorVm);
                }

                // Item reservation
                saleItem.Quantity -= cartItem.Quantity;
                _saleItemsRepository.Update(saleItem.Id, saleItem);

                var orderItem = new OrderItem
                {
                    Quantity = cartItem.Quantity,
                    SaleItemId = cartItem.SaleItemId,
                    Price = saleItem.SellingPrice
                };
                order.OrderedItems.Add(orderItem);
            }

            AddOrderState(order, OrderStateEnum.AwaitingPayment, "Заказ зарезервирован");
            var createdOrderId = _ordersRepository.Create(order);

            _databaseTransactionsManagementService.CommitTransaction();
            isTransactionCommitted = true;

            return createdOrderId;
        }
        finally
        {
            if (!isTransactionCommitted)
            {
                _databaseTransactionsManagementService.RollbackTransaction();
            }
        }
    }

    public OrderVm GetOrder(Guid orderId)
    {
        var order = _ordersRepository.Read(orderId);
        var orderVm = _mapper.Map<OrderVm>(order);

        return orderVm;
    }

    public void UpdateOrder(Guid orderId, UpdateOrderStateVm model)
    {
        throw new NotImplementedException();
    }

    public void DeleteOrder(Guid orderId)
    {
        _ordersRepository.Delete(orderId);
    }

    public PagedModel<OrderVm> SearchWithPagination(SearchEngineFilterVm? filterVm, int page, int pageSize)
    {
        var filter = _mapper.Map<SearchEngineFilter>(filterVm);
        var pagedModel = _ordersRepository.SearchWithPagination(filter, page, pageSize);
        var mappedPagedModel = pagedModel.MapTo<OrderVm>();

        return mappedPagedModel;
    }

    private void AddOrderState(Order order, OrderStateEnum newState, string enterDescription)
    {
        var previousStateSerialNumber = order.OrderStateHierarchical.Max(x => x.SerialNumber);
        
        order.OrderStateHierarchical.Add(new OrderStateHierarchicalItem
        {
            SerialNumber = previousStateSerialNumber + 1,
            State = newState,
            EnterDescription = enterDescription,
            EnteredDateTimeUtc = DateTime.UtcNow
        });
    }
}
