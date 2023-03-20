using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public record CustomerUpdateVm(
    [Required]
    string Name,
    [Required]
    string MobilePhone
);
