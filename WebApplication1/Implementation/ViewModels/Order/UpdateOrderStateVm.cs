using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Implementation.ViewModels.Order;

public record UpdateOrderStateVm(
    [Required]
    Guid OrderId,
    [Required]
    OrderStateEnum NewOrderState,
    string? EnterDescription,
    string? Details
);
