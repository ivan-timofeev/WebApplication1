using WebApplication1.Models;
using WebApplication1.Abstractions.Services;

namespace WebApplication1.Services.OrdersManagementService.UpdateOrderStateStrategies;

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
