using System.ComponentModel.DataAnnotations;
using WebApplication1.ViewModels;

namespace WebApplication1.Implementation.ViewModels.Order;

public record UpdateOrderPositionVm(
    [Required]
    Guid OrderId,
    [Required]
    Guid ProductId,
    [Required] [Range(0, 10000, ErrorMessage = "Use value in the range [0, 10000].")]
    int Quantity
);
