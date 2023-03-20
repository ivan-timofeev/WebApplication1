using WebApplication1.Models;

namespace WebApplication1.ViewModels;

public record SaleItemVm(
    ProductVm Product,
    int Quantity,
    decimal SellingPrice,
    decimal? PurchasePrice
);
