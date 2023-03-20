using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.ViewModels;

public record SalePointUpdateVm(
    [Required]
    string Name,
    string? Address,
    IEnumerable<SaleItemCreateVm>? SaleItems);
