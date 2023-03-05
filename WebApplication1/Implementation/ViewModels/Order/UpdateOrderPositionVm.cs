using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public record UpdateOrderPositionVm(
    [Required]
    Guid ProductId,
    [Required] [Range(0, 10000, ErrorMessage = "Use value in the range [0, 10000].")]
    int Quantity
);
