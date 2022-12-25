using System.ComponentModel.DataAnnotations;
using WebApplication1.ViewModels;

namespace WebApplication1.Implementation.ViewModels.Order;

public record CreateOrderVm(
    [Required]
    Guid CustomerId,
    [Required]
    Guid SalePointId,
    [Required]
    IEnumerable<SaleItemCreateVm> OrderedItems
);
