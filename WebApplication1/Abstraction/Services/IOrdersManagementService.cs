using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Services;

public interface IOrdersManagementService
{
    Order CreateOrder(CreateOrderVm model);
    Order GetOrder(Guid orderId);
    Order UpdateOrderPosition(UpdateOrderPositionVm model);
    Order UpdateOrderState(UpdateOrderStateVm model);
    void DeleteOrder(Guid orderId);
}
