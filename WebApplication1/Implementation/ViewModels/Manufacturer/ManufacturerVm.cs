namespace WebApplication1.ViewModels.Manufacturer;

public record ManufacturerVm(
    Guid Id,
    DateTime CreatedDateTimeUtc,
    DateTime? UpdatedDateTimeUtc,
    string Name
);
