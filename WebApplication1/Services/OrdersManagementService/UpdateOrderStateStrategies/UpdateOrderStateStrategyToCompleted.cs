using WebApplication1.Models;
using WebApplication1.Abstractions.Services;

namespace WebApplication1.Services.OrdersManagementService.UpdateOrderStateStrategies;

public class UpdateOrderStateStrategyToCompleted
    : IUpdateOrderStateStrategy
{
    public int Priority => 1;
    public OrderStateEnum[] FromStates => new[]
    {
        OrderStateEnum.DeliveryInProgress,
        OrderStateEnum.AwaitingCustomerPickup
    };
    public OrderStateEnum ToState => OrderStateEnum.Completed;

    public void UpdateOrder(Order order)
    {
        OrderStateUtils.AddOrderState(order, OrderStateEnum.Completed, 
            enterDescription: "Order successfully completed.");
    }
}
