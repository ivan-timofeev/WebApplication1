using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class SalePoint : DomainModel
{
    public string Name { get; set; }
    public string? Address { get; set; }

    public ICollection<SaleItem>? SaleItems { get; set; }
}
