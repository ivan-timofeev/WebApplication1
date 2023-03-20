namespace WebApplication1.Models;

public class ShoppingCartItem : DomainModel
{
    public Guid SaleItemId { get; set; }
    public SaleItem SaleItem { get; set; }
    public int OrderNumber { get; set; }
    public int Quantity { get; set; }
    public int AvailableQuantity { get; set; }

    public ShoppingCartItem()
    {
        SaleItem = null!;
    }
}
