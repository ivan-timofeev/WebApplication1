using WebApplication1.Implementation.ViewModels.Order;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.Customer;

// Page PersonalArea/Index.js data contract
public record PersonalAreaVm(
    CustomerVm Customer,
    IEnumerable<CustomerOrderVm> CustomerOrders
);

public record CustomerOrderVm(
    Guid Id,
    CustomerOrderSalePointVm SalePoint,
    OrderStateEnum ActualOrderState,
    IEnumerable<OrderStateHierarchicalItemVm> OrderStateHierarchical,
    IEnumerable<CustomerOrderItemVm> OrderedItems);

public record CustomerOrderItemVm(
    Guid ProductId,
    int Quantity,
    decimal Price
);

public record CustomerOrderSalePointVm(
    Guid Id,
    string Name,
    string? Address
);
