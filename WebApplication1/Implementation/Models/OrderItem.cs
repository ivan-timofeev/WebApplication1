using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class OrderItem : DomainModel
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    public Guid SalePointId { get; set; }
    public SalePoint SalePoint { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public OrderItem()
    {
        Order = null!;
        Product = null!;
        SalePoint = null!;
    }
}
