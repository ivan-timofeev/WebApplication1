using WebApplication1.Common.Exceptions;
using WebApplication1.ViewModels;

namespace WebApplication1.Abstraction.Services;

public interface IShoppingCartsManagementService
{
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    ShoppingCartVm GetShoppingCart(Guid cartId);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    ShoppingCartVm GetShoppingCartByCustomer(Guid customerId);
    Guid CreateCart(Guid customerId);
    Guid CreateCart(Guid customerId, ShoppingCartVm model);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    void UpdateCartItem(Guid customerId, ShoppingCartItemUpdateVm model);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    void RefreshCart(Guid cartId);
    /// <exception cref="EntityNotFoundInTheDatabaseException"></exception>
    void DeleteCart(Guid cartId);
}
