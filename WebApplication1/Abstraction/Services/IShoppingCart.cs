using System.ComponentModel.DataAnnotations;
using WebApplication1.Abstraction.Models;
using WebApplication1.Models;

namespace WebApplication1.Abstraction.Services;

public interface IShoppingCartsManagementService
{
    ShoppingCartVm GetShoppingCart(Guid customerId);
    void StartNewShoppingSession(Guid customerId);
    void StartNewShoppingSession(Guid customerId, ShoppingCartVm model);
    void UpdateItem(Guid customerId, ShoppingCartItemUpdateVm model);
    void ReleaseCart(Guid customerId);
}

public class ShoppingCartsManagementService : IShoppingCartsManagementService
{
    public ShoppingCartVm GetShoppingCart(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public void StartNewShoppingSession(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public void StartNewShoppingSession(Guid customerId, ShoppingCartVm model)
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(Guid customerId, ShoppingCartItemUpdateVm model)
    {
        throw new NotImplementedException();
    }

    public void ReleaseCart(Guid customerId)
    {
        throw new NotImplementedException();
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
