namespace WebApplication1.Abstraction.Models;

public interface IDomainModel
{
    Guid Id { get; }
    DateTime CreatedDateTimeUtc { get; }
    DateTime? UpdatedDateTimeUtc { get; }
}
