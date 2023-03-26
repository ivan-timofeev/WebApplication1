using WebApplication1.Models;
using WebApplication1.Abstractions.Services;

namespace WebApplication1.Services.OrdersManagementService.UpdateOrderStateStrategies;

public class UpdateOrderStateStrategyToAwaitingCustomerPickup
    : IUpdateOrderStateStrategy
{
    public int Priority => 1;
    public OrderStateEnum[] FromStates => new[] { OrderStateEnum.Assembling };
    public OrderStateEnum ToState => OrderStateEnum.AwaitingCustomerPickup;

    public void UpdateOrder(Order order)
    {
        OrderStateUtils.AddOrderState(order, OrderStateEnum.AwaitingCustomerPickup, 
            enterDescription: "Order collected.");
    }
}
