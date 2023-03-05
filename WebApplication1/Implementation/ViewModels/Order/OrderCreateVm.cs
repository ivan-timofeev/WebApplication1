using System.ComponentModel.DataAnnotations;
using WebApplication1.ViewModels;

namespace WebApplication1.ViewModels;

public record OrderCreateVm(
    [Required]
    Guid CustomerId,
    [Required]
    Guid SalePointId
);
