namespace WebApplication1.Abstraction.Models;

public abstract class DomainModel : IDomainModel
{
    public Guid Id { get; set; }
    public DateTime CreatedDateTimeUtc { get; set; }
    public DateTime? UpdatedDateTimeUtc { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedDateTimeUtc { get; set; }
}
