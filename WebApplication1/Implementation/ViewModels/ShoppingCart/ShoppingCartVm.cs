namespace WebApplication1.ViewModels;

public record ShoppingCartVm
(
    Guid CustomerId,
    IEnumerable<ShoppingCartItemVm> CartItems
);
