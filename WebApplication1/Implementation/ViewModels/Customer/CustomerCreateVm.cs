using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.Customer;

public record CustomerCreateVm(
    [Required]
    string Name,
    [Required]
    string MobilePhone
);
