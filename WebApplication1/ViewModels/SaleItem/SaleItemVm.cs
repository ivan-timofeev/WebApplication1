using WebApplication1.Models;

namespace WebApplication1.ViewModels;

public record SaleItemVm(
    Guid Id,
    ProductVm Product,
    int Quantity,
    decimal SellingPrice,
    decimal? PurchasePrice
);
