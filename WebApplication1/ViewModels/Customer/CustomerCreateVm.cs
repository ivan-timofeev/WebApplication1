using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public record CustomerCreateVm(
    [Required]
    string Name,
    [Required]
    string MobilePhone
);
