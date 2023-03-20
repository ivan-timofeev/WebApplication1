namespace WebApplication1.Models;


public class ShoppingCart : DomainModel
{
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public List<ShoppingCartItem> CartItems { get; set; }

    public ShoppingCart()
    {
        Customer = null!;
        CartItems = new List<ShoppingCartItem>();
    }
}
