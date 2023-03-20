namespace WebApplication1.ViewModels;

public record ManufacturerVm(
    Guid Id,
    DateTime CreatedDateTimeUtc,
    DateTime? UpdatedDateTimeUtc,
    string Name
);
