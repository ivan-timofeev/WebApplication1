using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public record ShoppingCartItemUpdateVm
(
    [Required]
    Guid SaleItemId,
    [Range(0, 1000, ErrorMessage = "Value must be in the range [0, 1000]")]
    int Quantity
);
