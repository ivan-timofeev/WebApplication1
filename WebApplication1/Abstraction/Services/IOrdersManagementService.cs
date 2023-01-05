using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Services;

public interface IOrdersManagementService
{
    Order CreateOrder(OrderCreateVm model);
    Order GetOrder(Guid orderId);
    Order UpdateOrderPosition(Guid orderId, UpdateOrderPositionVm model);
    Order UpdateOrderState(Guid orderId, UpdateOrderStateVm model);
    void DeleteOrder(Guid orderId);
}
