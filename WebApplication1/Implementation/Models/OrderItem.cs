using WebApplication1.Abstraction.Models;

namespace WebApplication1.Models;

public class OrderItem : DomainModel
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid SaleItemId { get; set; }
    public SaleItem SaleItem { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public OrderItem()
    {
        Order = null!;
        SaleItem = null!;
    }
}
