using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels.Manufacturer;

public record ManufacturerCreateVm(
    [Required]
    string Name
);
