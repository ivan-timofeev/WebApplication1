namespace WebApplication1.ViewModels;

public record ProductUpdateVm(
    string Name,
    string? Description,
    decimal? DefaultPrice,
    IEnumerable<ProductCharacteristicVm>? ProductCharacteristics
);