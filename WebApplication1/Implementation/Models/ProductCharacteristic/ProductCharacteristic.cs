using WebApplication1.Abstraction.Models;
using WebApplication1.Models;

namespace WebApplication1.Models;

public abstract class ProductCharacteristic : DomainModel
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public string Name { get; set; }
}