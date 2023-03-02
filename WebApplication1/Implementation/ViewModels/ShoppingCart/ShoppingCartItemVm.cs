namespace WebApplication1.ViewModels.ShoppingCart;

public record ShoppingCartItemVm
(
    Guid SaleItemId,
    int Quantity,
    int AvailableQuantity
);
