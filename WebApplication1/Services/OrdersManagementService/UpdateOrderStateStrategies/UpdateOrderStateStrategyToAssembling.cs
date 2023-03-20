using WebApplication1.Models;
using WebApplication1.Abstractions.Services;

namespace WebApplication1.Services.OrdersManagementService.UpdateOrderStateStrategies;

class UpdateOrderStateStrategyToAssembling
    : IUpdateOrderStateStrategy
{
    public int Priority => 1;
    public OrderStateEnum[] FromStates => new[] { OrderStateEnum.AwaitingPayment };
    public OrderStateEnum ToState => OrderStateEnum.Assembling;

    public void UpdateOrder(Order order)
    {
        OrderStateUtils.AddOrderState(order, OrderStateEnum.Assembling, 
            enterDescription: "Order payed.");
    }
}
