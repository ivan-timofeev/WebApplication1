namespace WebApplication1.Models;

public class Product : DomainModel
{
    public string? Name { get; set; }
    public string? ImageUri { get; set; }
    public string? Description { get; set; }
    public ICollection<ProductCharacteristic>? ProductCharacteristics { get; set; }
}
