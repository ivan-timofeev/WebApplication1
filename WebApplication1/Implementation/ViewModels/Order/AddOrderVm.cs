using System.ComponentModel.DataAnnotations;
using WebApplication1.ViewModels;

namespace WebApplication1.Implementation.ViewModels.Order;

public record AddToOrderVm(
    [Required]
    Guid ProductId,
    [Required]
    int Quantity
);