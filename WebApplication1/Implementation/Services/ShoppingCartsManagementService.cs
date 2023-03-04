using AutoMapper;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Services;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services;

public class ShoppingCartsManagementService : IShoppingCartsManagementService
{
    private readonly IShoppingCartsRepository _shoppingCartsRepository;
    private readonly IMapper _mapper;

    public ShoppingCartsManagementService(
        IShoppingCartsRepository shoppingCartsRepository,
        IMapper mapper)
    {
        _shoppingCartsRepository = shoppingCartsRepository;
        _mapper = mapper;
    }

    public ShoppingCartVm GetShoppingCart(Guid cartId)
    {
        var shoppingCart = _shoppingCartsRepository.Read(cartId);
        var viewModel = _mapper.Map<ShoppingCartVm>(shoppingCart);

        return viewModel;
    }

    public ShoppingCartVm GetShoppingCartByCustomer(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public Guid CreateCart(Guid customerId)
    {
        var shoppingCart = new ShoppingCart
        {
            CustomerId = customerId
        };
        var createdShoppingCartId = _shoppingCartsRepository.Create(shoppingCart);

        return createdShoppingCartId;
    }

    public Guid CreateCart(Guid customerId, ShoppingCartVm model)
    {
        var shoppingCart = _mapper.Map<ShoppingCart>(model);
        _shoppingCartsRepository.Create(shoppingCart);
        
        throw new NotImplementedException();
    }

    public void UpdateCartItem(Guid customerId, ShoppingCartItemUpdateVm model)
    {
        _shoppingCartsRepository.UpdateShoppingCartItem(
            customerId, model.SaleItemId, model.Quantity);
    }

    public void RefreshCart(Guid cartId)
    {
        var shoppingCart = _shoppingCartsRepository.Read(cartId);
        _shoppingCartsRepository.Update(cartId, shoppingCart);
    }

    public void DeleteCart(Guid cartId)
    {
        _shoppingCartsRepository.Delete(cartId);
    }
}
