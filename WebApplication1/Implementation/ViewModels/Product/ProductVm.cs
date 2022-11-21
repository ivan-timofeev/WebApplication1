namespace WebApplication1.ViewModels;

public record ProductVm(
    Guid Id,
    DateTime CreatedDateTimeUtc,
    DateTime? UpdatedDateTimeUtc,
    string Name,
    string? Description,
    decimal? DefaultPrice,
    IEnumerable<ProductCharacteristicVm>? ProductCharacteristics
);