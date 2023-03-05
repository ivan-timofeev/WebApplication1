namespace WebApplication1.ViewModels;

public record ShoppingCartItemVm
(
    Guid SaleItemId,
    int Quantity,
    int AvailableQuantity
);
