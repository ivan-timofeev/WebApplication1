using System.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Abstraction.Services;
using WebApplication1.Data;
using WebApplication1.Data.Repositories;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class OrdersManagementService : IOrdersManagementService
{
    private readonly IMapper _mapper;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IRepository<Customer> _customersRepository;
    private readonly IRepository<SalePoint> _salePointsRepository;
    private readonly WebApplicationDbContext _dbContext;

    public OrdersManagementService(
        IMapper mapper,
        IOrdersRepository ordersRepository,
        IRepository<Customer> customersRepository,
        IRepository<SalePoint> salePointsRepository,
        WebApplicationDbContext dbContext)
    {
        _mapper = mapper;
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
                    Description = "Заказ создан"
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

    public Order UpdateOrderState(UpdateOrderVm model)
    {
        /*
         * TO DO:
         * 
         * Нужно сделать что то типа ордер стейт процессора
         * процессор должен брать стратегию на основании текущего и следующего стейтов
         * брать стратегию и вызывать её, передав в нее аргументы (строку с описанием)
         *
         * OrderStateProcessingStrategy:
         *     + Свойство State Source
         *     + Свойство State Destination
         *     + Метод void ExecuteStrategy(string Description)
         *
         * OrderStateStrategyResolver:
         *     + Метод GetStrategy(State source, State destination)
         *     + CTOR: (IEnumerable<OrderStateProcessingStrategy> registeredStrategies) 
         */
        throw new NotImplementedException();
    }

    public void DeleteOrder(Guid orderId)
    {
        throw new NotImplementedException();
    }
}