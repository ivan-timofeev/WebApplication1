using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Abstraction.Services;

public interface IShoppingCart
{
    ShoppingCartVm GetShoppingCart(Guid customerId);
    void StartNewShoppingSession(Guid customerId);
    void StartNewShoppingSession(Guid customerId, ShoppingCartVm model);
    void UpdateItem(Guid customerId, ShoppingCartUpdateItemVm model);
}

public record ShoppingCartUpdateItemVm
(
    [Required]
    Guid SalePointId,
    [Required]
    Guid SaleItemId,
    [Range(0, 1000, ErrorMessage = "Value must be in the range [0, 1000]")]
    int Quantity
);

public record ShoppingCartVm
(
    Guid CustomerId,
    IEnumerable<ShoppingCartItemVm> CartItems
);

public record ShoppingCartItemVm
(
    Guid SalePointId,
    Guid SaleItemId,
    int Quantity
);
