using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
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
    private readonly WebApplicationDbContext _dbContext;

    public OrdersManagementService(
        IOrdersRepository ordersRepository,
        ICustomersRepository customersRepository,
        ISalePointsRepository salePointsRepository,
        WebApplicationDbContext dbContext)
    {
        _ordersRepository = ordersRepository;
        _customersRepository = customersRepository;
        _salePointsRepository = salePointsRepository;
        _dbContext = dbContext;
    }


    public Order CreateOrder(CreateOrderVm model)
    {
        var customer = _customersRepository.Read(model.CustomerId);
        var salePoint = _salePointsRepository.Read(model.SalePointId);

        var order = _ordersRepository.Create(new Order
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

        return order;
    }

    public Order GetOrder(Guid orderId)
    {
        return _ordersRepository.Read(orderId);
    }

    public Order UpdateOrderPosition(UpdateOrderPositionVm model)
    {
        _dbContext.Database.BeginTransaction(IsolationLevel.Serializable);
        var isTransactionCommitted = false;

        try
        {
            var order = _ordersRepository.Read(model.OrderId);
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
            
            _dbContext.Database.CommitTransaction();
            isTransactionCommitted = true;

            return order;
        }
        finally
        {
            if (!isTransactionCommitted)
            {
                _dbContext.Database.RollbackTransaction();
            }
        }
    }

    public Order UpdateOrderState(UpdateOrderStateVm model)
    {
        _dbContext.Database.BeginTransaction(IsolationLevel.Serializable);
        var isTransactionCommitted = false;
        
        try
        {
            var order = _ordersRepository.Read(model.OrderId);
            var previousStateSerialNumber = order.OrderStateHierarchical.Max(x => x.SerialNumber);

            order.OrderStateHierarchical.Add(new OrderStateHierarchicalItem
            {
                SerialNumber = previousStateSerialNumber + 1,
                EnteredDateTimeUtc = DateTime.UtcNow,
                State = model.NewOrderState,
                EnterDescription = model.EnterDescription,
                Details = model.Details
            });

            _ordersRepository.Update(order.Id, order);
            
            _dbContext.Database. CommitTransaction();
            isTransactionCommitted = true;

            return order;
        }
        finally
        {
            if (!isTransactionCommitted)
            {
                _dbContext.Database.RollbackTransaction();
            }
        }
    }

    public void DeleteOrder(Guid orderId)
    {
        _ordersRepository.Delete(orderId);
    }
}