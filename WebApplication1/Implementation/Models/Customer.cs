using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class Customer : DomainModel
{
    public string Name { get; set; }
    public string MobilePhone { get; set; }
    public ICollection<Order>? Orders { get; set; }
}
