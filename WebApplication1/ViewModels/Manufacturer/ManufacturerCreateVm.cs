using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels;

public record ManufacturerCreateVm(
    [Required]
    string Name
);
