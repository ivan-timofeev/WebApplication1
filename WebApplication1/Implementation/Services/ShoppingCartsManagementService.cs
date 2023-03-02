using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Services;
using WebApplication1.Common.Exceptions;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels.ShoppingCart;

namespace WebApplication1.Services;

public class ShoppingCartsManagementService : IShoppingCartsManagementService
{
    private readonly IShoppingCartsRepository _shoppingCartsRepository;

    public ShoppingCartsManagementService(
        IShoppingCartsRepository shoppingCartsRepository)
    {
        _shoppingCartsRepository = shoppingCartsRepository;
    }

    public ShoppingCart GetShoppingCart(Guid id)
    {
        var shoppingCart = _shoppingCartsRepository.Read(id);

        return shoppingCart;
    }

    public void StartNewShoppingSession(Guid customerId)
    {
        var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(x => x.CustomerId == customerId);

        if (shoppingCart != null)
        {
            shoppingCart.CartItems = new List<ShoppingCartItem>();
        }
        else
        {
            _dbContext.ShoppingCarts.Add(new ShoppingCart
            {
                CartItems = new List<ShoppingCartItem>(),
                CustomerId = customerId
            });
        }
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
