using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels;

public record UpdateOrderStateVm(
    [Required]
    OrderStateEnum NewOrderState,
    string? EnterDescription,
    string? Details
);
