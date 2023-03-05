using WebApplication1.Models;
using WebApplication1.ViewModels.Customer;

namespace WebApplication1.ViewModels;

public record OrderVm
(
    Guid Id,
    CustomerVm Customer,
    OrderStateEnum ActualOrderState,
    IEnumerable<OrderItemVm> OrderedItems,
    IEnumerable<OrderStateHierarchicalItemVm> OrderStateHierarchical
);

public record OrderItemVm
(
    OrderItemSaleItemVm SaleItem,
    int Quantity,
    decimal Price
);

public record OrderItemSaleItemVm
(
    ProductVm Product,
    SalePointVm SalePoint
);

public record OrderStateHierarchicalItemVm
(
    int SerialNumber,
    DateTime EnteredDateTimeUtc,
    OrderStateEnum State,
    string? EnterDescription,
    string? Details
);
