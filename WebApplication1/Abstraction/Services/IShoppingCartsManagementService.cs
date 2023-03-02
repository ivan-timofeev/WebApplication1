using WebApplication1.Models;
using WebApplication1.ViewModels.ShoppingCart;

namespace WebApplication1.Abstraction.Services;

public interface IShoppingCartsManagementService
{
    ShoppingCart GetShoppingCart(Guid id);
    Guid StartNewShoppingSession(Guid customerId);
    Guid StartNewShoppingSession(Guid customerId, ShoppingCartVm model);
    void UpdateItem(Guid customerId, ShoppingCartItemUpdateVm model);
    void RefreshCart();
    void ReleaseCart(Guid id);
}
