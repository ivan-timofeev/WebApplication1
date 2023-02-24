using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Services;

public interface IOrdersManagementService
{
    Guid CreateOrder(OrderCreateVm model);
    Order GetOrder(Guid orderId);
    void UpdateOrderPosition(Guid orderId, UpdateOrderPositionVm model);
    void UpdateOrderState(Guid orderId, UpdateOrderStateVm model);
    void DeleteOrder(Guid orderId);
}
