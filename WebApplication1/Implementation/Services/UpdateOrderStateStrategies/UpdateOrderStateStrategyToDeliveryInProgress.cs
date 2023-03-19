using WebApplication1.Abstraction.Services;
using WebApplication1.Models;

namespace WebApplication1.Services.UpdateOrderStateStrategies;

class UpdateOrderStateStrategyToDeliveryInProgress
    : IUpdateOrderStateStrategy
{
    public int Priority => 1;
    public OrderStateEnum[] FromStates => new[] { OrderStateEnum.AwaitingForDelivery };
    public OrderStateEnum ToState => OrderStateEnum.DeliveryInProgress;

    public void UpdateOrder(Order order)
    {
        OrderStateUtils.AddOrderState(order, OrderStateEnum.DeliveryInProgress, 
            enterDescription: "Order taken for delivery.");
    }
}
