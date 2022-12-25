using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels;

public record SaleItemCreateVm(
    [Required]
    Guid ProductId,
    [Required]
    int Quantity,
    [Required]
    decimal SellingPrice,
    decimal? PurchasePrice
);
