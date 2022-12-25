using WebApplication1.Abstraction.Models;
using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;

namespace WebApplication1.Implementation.Services;

public interface IOrdersManagementService
{
    Order CreateOrder(CreateOrderVm model);
    Order GetOrder(Guid orderId);
    void SetOrderState(Guid orderId, OrderStateEnum newOrderState, string? description = null);
}

public class OrdersManagementService : IOrdersManagementService
{
    private readonly IRepository<Order> _ordersRepository;
    private readonly IRepository<Customer> _customersRepository;

    public OrdersManagementService(
        IRepository<Order> ordersRepository,
        IRepository<Customer> customersRepository)
    {
        _ordersRepository = ordersRepository;
        _customersRepository = customersRepository;
    }
}