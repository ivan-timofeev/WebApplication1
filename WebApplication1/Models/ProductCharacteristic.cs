namespace WebApplication1.Models;

public abstract class ProductCharacteristic : DomainModel
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public string Name { get; set; }

    protected ProductCharacteristic()
    {
        Product = null!;
        Name = string.Empty;
    }
}
