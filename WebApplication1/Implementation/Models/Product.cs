using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class Product : DomainModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? DefaultPrice { get; set; }
    public ICollection<ProductCharacteristic>? ProductCharacteristics { get; set; }
}
