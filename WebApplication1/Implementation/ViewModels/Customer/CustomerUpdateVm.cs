using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.Customer;

public record CustomerUpdateVm(
    [Required]
    string Name,
    [Required]
    string MobilePhone
);
