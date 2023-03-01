using System.ComponentModel.DataAnnotations;
using WebApplication1.Abstraction.Models;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Services;

public interface IShoppingCart
{
    ShoppingCartVm GetShoppingCart(Guid customerId);
    void StartNewShoppingSession(Guid customerId);
    void StartNewShoppingSession(Guid customerId, ShoppingCartVm model);
    void UpdateItem(Guid customerId, ShoppingCartItemUpdateVm model);
    void ReleaseCart(Guid customerId);
}

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

public record ShoppingCartItemUpdateVm
(
    [Required]
    Guid SaleItemId,
    [Range(0, 1000, ErrorMessage = "Value must be in the range [0, 1000]")]
    int Quantity
);

public record ShoppingCartVm
(
    Guid CustomerId,
    IEnumerable<ShoppingCartItemVm> CartItems
);

public record ShoppingCartItemVm
(
    Guid SaleItemId,
    int Quantity,
    int AvailableQuantity
);
