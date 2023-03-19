using WebApplication1.Abstraction.Services;
using WebApplication1.Models;

namespace WebApplication1.Services.UpdateOrderStateStrategies;

class UpdateOrderStateStrategyToAwaitingForDelivery
    : IUpdateOrderStateStrategy
{
    public int Priority => 1;
    public OrderStateEnum[] FromStates => new[] { OrderStateEnum.Assembling };
    public OrderStateEnum ToState => OrderStateEnum.AwaitingForDelivery;

    public void UpdateOrder(Order order)
    {
        OrderStateUtils.AddOrderState(order, OrderStateEnum.AwaitingForDelivery, 
            enterDescription: "Order collected.");
    }
}
