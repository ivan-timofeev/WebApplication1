using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public record ManufacturerUpdateVm(
    [Required]
    string Name
);
