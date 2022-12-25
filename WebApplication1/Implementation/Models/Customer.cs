using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class Customer : DomainModel
{
    public ICollection<Order>? Orders { get; set; }
}
