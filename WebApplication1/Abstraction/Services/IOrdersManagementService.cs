using WebApplication1.Abstraction.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.SearchEngine;

namespace WebApplication1.Abstraction.Services;

public interface IOrdersManagementService
{
    /// <summary>
    /// Creates an order from the shopping cart with items reservation.
    /// </summary>
    Guid CreateOrderFromCustomerShoppingCart(Guid customerId);
    OrderVm GetOrder(Guid orderId);
    void UpdateOrder(Guid orderId, UpdateOrderStateVm model);
    void DeleteOrder(Guid orderId);
    PagedModel<OrderVm> SearchWithPagination(SearchEngineFilterVm? filter, int page, int pageSize);
}
