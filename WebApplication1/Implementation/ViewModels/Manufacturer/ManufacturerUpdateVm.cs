using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.Manufacturer;

public record ManufacturerUpdateVm(
    [Required]
    string Name
);
