using System.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Services;
using WebApplication1.Data;
using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class OrdersManagementService : IOrdersManagementService
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ICustomersRepository _customersRepository;
    private readonly ISalePointsRepository _salePointsRepository;
    private readonly IDatabaseTransactionsManagementService _databaseTransactionsManagementService;

    public OrdersManagementService(
        IOrdersRepository ordersRepository,
        ICustomersRepository customersRepository,
        ISalePointsRepository salePointsRepository,
        IDatabaseTransactionsManagementService databaseTransactionsManagementService)
    {
        _ordersRepository = ordersRepository;
        _customersRepository = customersRepository;
        _salePointsRepository = salePointsRepository;
        _databaseTransactionsManagementService = databaseTransactionsManagementService;
    }


    public Guid CreateOrder(OrderCreateVm model)
    {
        var customer = _customersRepository.Read(model.CustomerId);
        var salePoint = _salePointsRepository.Read(model.SalePointId);

        var orderId = _ordersRepository.Create(new Order
        {
            OrderStateHierarchical = new List<OrderStateHierarchicalItem>()
            {
                new OrderStateHierarchicalItem()
                {
                    SerialNumber = 1,
                    EnteredDateTimeUtc = DateTime.UtcNow,
                    State = OrderStateEnum.Creating,
                    EnterDescription = "Заказ создан"
                }
            },
            CustomerId = customer.Id,
            Customer = customer,
            SalePointId = salePoint.Id,
            SalePoint = salePoint
        });

        return orderId;
    }

    public Order GetOrder(Guid orderId)
    {
        return _ordersRepository.Read(orderId);
    }

    public void UpdateOrderPosition(Guid orderId, UpdateOrderPositionVm model)
    {
        _databaseTransactionsManagementService.BeginTransaction(IsolationLevel.Serializable);
        var isTransactionCommitted = false;

        try
        {
            var order = _ordersRepository.Read(orderId);

            if (order.ActualOrderState != OrderStateEnum.Creating)
                throw new Exception("Данный заказ уже был сформирован. Редактировать его элементы НЕЛЬЗЯ");
            
            var salePoint = _salePointsRepository.Read(order.SalePointId);
            var saleItem = salePoint.SaleItems.FirstOrDefault(x => x.ProductId == model.ProductId);

            // TODO: create new exceptions
            if (saleItem is null)
                throw new Exception("В торговой точке нет такого товара.");
            if (model.Quantity > saleItem.Quantity)
                throw new Exception("В торговой точке нет столько товара.");

            var orderItem = order.OrderedItems.FirstOrDefault(x => x.ProductId == model.ProductId);

            if (orderItem != null)
            {
                saleItem.Quantity += orderItem.Quantity;
                order.OrderedItems.Remove(orderItem);
            }

            if (model.Quantity != 0)
            {
                var updatedOrderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = saleItem.ProductId,
                    SalePointId = salePoint.Id,
                    Price = saleItem.SellingPrice,
                    Quantity = model.Quantity
                };
                saleItem.Quantity -= updatedOrderItem.Quantity;
                order.OrderedItems.Add(updatedOrderItem);
            }

            _salePointsRepository.Update(salePoint.Id, salePoint);
            _ordersRepository.Update(order.Id, order);
            
            _databaseTransactionsManagementService.CommitTransaction();
            isTransactionCommitted = true;
        }
        finally
        {
            if (!isTransactionCommitted)
            {
                _databaseTransactionsManagementService.RollbackTransaction();
            }
        }
    }

    public void UpdateOrderState(Guid orderId, UpdateOrderStateVm model)
    {
        _databaseTransactionsManagementService.BeginTransaction(IsolationLevel.Serializable);
        var isTransactionCommitted = false;
        
        try
        {
            var order = _ordersRepository.Read(orderId);
            var previousStateSerialNumber = order.OrderStateHierarchical.Max(x => x.SerialNumber);

            order.OrderStateHierarchical.Add(new OrderStateHierarchicalItem
            {
                SerialNumber = previousStateSerialNumber + 1,
                EnteredDateTimeUtc = DateTime.UtcNow,
                State = model.NewOrderState,
                EnterDescription = model.EnterDescription,
                Details = model.Details
            });
            
            if (model.NewOrderState == OrderStateEnum.Canceled && ItIsTheFirstCancellation(order))
                RemoveProductsReservation(order);

            _ordersRepository.Update(order.Id, order);
            
            _databaseTransactionsManagementService.CommitTransaction();
            isTransactionCommitted = true;
        }
        finally
        {
            if (!isTransactionCommitted)
            {
                _databaseTransactionsManagementService.RollbackTransaction();
            }
        }
    }

    public void DeleteOrder(Guid orderId)
    {
        _ordersRepository.Delete(orderId);
    }
    
    private static bool ItIsTheFirstCancellation(Order order)
    {
        return order.OrderStateHierarchical
                   .FirstOrDefault(x => x.State == OrderStateEnum.Canceled)
               is null;
    }
    
    private void RemoveProductsReservation(Order order)
    {
        var salePoint = _salePointsRepository.Read(order.SalePointId);
        
        foreach (var orderItem in order.OrderedItems)
        {
            var saleItem = salePoint.SaleItems.First(x => x.ProductId == orderItem.ProductId);
            saleItem.Quantity += orderItem.Quantity;
        }
    }
}
