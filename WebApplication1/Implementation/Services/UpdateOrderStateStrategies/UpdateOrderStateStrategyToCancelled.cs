using WebApplication1.Abstraction.Services;
using WebApplication1.Models;

namespace WebApplication1.Services.UpdateOrderStateStrategies;

class UpdateOrderStateStrategyToCancelled
    : IUpdateOrderStateStrategy
{
    public int Priority => 1;
    public OrderStateEnum[] FromStates => new[]
    {
        OrderStateEnum.Created,
        OrderStateEnum.AwaitingPayment,
        OrderStateEnum.Assembling,
        OrderStateEnum.AwaitingCustomerPickup,
        OrderStateEnum.AwaitingForDelivery,
        OrderStateEnum.DeliveryInProgress
    };
    public OrderStateEnum ToState => OrderStateEnum.Canceled;

    public void UpdateOrder(Order order)
    {
        // Здесь нужно убрать резервацию товаров
        
        throw new NotImplementedException();
    }
}
