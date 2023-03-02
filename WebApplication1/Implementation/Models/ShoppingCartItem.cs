using WebApplication1.Abstraction.Models;
using WebApplication1.Models;

namespace WebApplication1.Implementation.Models;

public class ShoppingCartItem : DomainModel
{
    public Guid SaleItemId { get; set; }
    public SaleItem SaleItem { get; set; }
    public int Quantity { get; set; }

    public ShoppingCartItem()
    {
        SaleItem = null!;
    }
}
