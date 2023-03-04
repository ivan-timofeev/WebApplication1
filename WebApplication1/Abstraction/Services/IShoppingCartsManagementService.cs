using WebApplication1.ViewModels;

namespace WebApplication1.Abstraction.Services;

public interface IShoppingCartsManagementService
{
    ShoppingCartVm GetShoppingCart(Guid cartId);
    ShoppingCartVm GetShoppingCartByCustomer(Guid customerId);
    Guid CreateCart(Guid customerId);
    Guid CreateCart(Guid customerId, ShoppingCartVm model);
    void UpdateCartItem(Guid customerId, ShoppingCartItemUpdateVm model);
    void RefreshCart(Guid cartId);
    void DeleteCart(Guid cartId);
}
