namespace WebApplication1.Abstractions.Models;

public interface IDomainModel
{
    Guid Id { get; }
    DateTime CreatedDateTimeUtc { get; }
    DateTime? UpdatedDateTimeUtc { get; }
}
