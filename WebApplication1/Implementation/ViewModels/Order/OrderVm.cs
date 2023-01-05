using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.Customer;

namespace WebApplication1.Implementation.ViewModels.Order;

public record OrderVm(
    Guid Id,
    CustomerVm Customer,
    SalePointVm SalePoint,
    OrderStateEnum ActualOrderState,
    IEnumerable<OrderStateHierarchicalItemVm> OrderStateHierarchical);

public record OrderStateHierarchicalItemVm(
    int SerialNumber,
    DateTime EnteredDateTimeUtc,
    OrderStateEnum State,
    string? EnterDescription,
    string? Details
);
