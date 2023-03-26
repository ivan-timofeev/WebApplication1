using WebApplication1.Models;
using WebApplication1.Abstractions.Services;

namespace WebApplication1.Services.OrdersManagementService.UpdateOrderStateStrategies;

public class UpdateOrderStateStrategyToDeliveryInProgress
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
