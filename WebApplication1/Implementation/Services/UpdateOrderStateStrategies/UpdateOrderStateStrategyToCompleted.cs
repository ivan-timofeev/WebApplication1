using WebApplication1.Abstraction.Services;
using WebApplication1.Models;

namespace WebApplication1.Services.UpdateOrderStateStrategies;

class UpdateOrderStateStrategyToCompleted
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
