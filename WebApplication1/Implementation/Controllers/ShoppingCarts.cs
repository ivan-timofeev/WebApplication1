using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Abstraction.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartsController : ControllerBase
{
    private readonly IShoppingCartsManagementService _shoppingCartsManagementService;

    public ShoppingCartsController(
        IShoppingCartsManagementService shoppingCartsManagementService )
    {
        _shoppingCartsManagementService = shoppingCartsManagementService;
    }

    [HttpPost("{customerId:guid}")]
    public IActionResult CreateShoppingCart(
        Guid customerId,
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        ShoppingCartVm? model = null)
    {
        var createdShoppingCartId = model is null
            ? _shoppingCartsManagementService.CreateCart(customerId)
            : _shoppingCartsManagementService.CreateCart(customerId, model);

        return CreatedAtAction(
            nameof(Get),
            routeValues: new { shoppingCartId = createdShoppingCartId },
            value: new { ShoppingCartId = createdShoppingCartId});
    }

    [HttpGet("{shoppingCartId:guid}")]
    public IActionResult Get(ShoppingCartSelectModeEnum selectMode, Guid shoppingCartId)
    {
        var shoppingCartVm = selectMode == ShoppingCartSelectModeEnum.SelectByShoppingCartId
            ? _shoppingCartsManagementService.GetShoppingCart(shoppingCartId)
            : _shoppingCartsManagementService.GetShoppingCartByCustomer(shoppingCartId);

        return Ok(shoppingCartVm);
    }

    [HttpPut("{shoppingCartId:guid}")]
    public IActionResult UpdateShoppingCartItem(Guid shoppingCartId, ShoppingCartItemUpdateVm model)
    {
        _shoppingCartsManagementService.UpdateCartItem(shoppingCartId, model);

        return Accepted();
    }

    [HttpDelete("{shoppingCartId:guid}")]
    public IActionResult Delete(Guid shoppingCartId)
    {
        _shoppingCartsManagementService.DeleteCart(shoppingCartId);

        return Accepted();
    }
}
