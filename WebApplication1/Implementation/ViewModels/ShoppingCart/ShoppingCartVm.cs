namespace WebApplication1.ViewModels.ShoppingCart;

public record ShoppingCartVm
(
    Guid CustomerId,
    IEnumerable<ShoppingCartItemVm> CartItems
);