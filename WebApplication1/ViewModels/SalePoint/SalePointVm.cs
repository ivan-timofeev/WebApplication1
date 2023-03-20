namespace WebApplication1.ViewModels;

public record SalePointVm(
    Guid Id,
    DateTime CreatedDateTimeUtc,
    DateTime? UpdatedDateTimeUtc,
    string Name,
    string? Address,
    IEnumerable<SaleItemVm> SaleItems);
