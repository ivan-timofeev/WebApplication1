using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Abstractions.Services;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductsManagementService _productsManagementService;

    public ProductsController(
        ILogger<ProductsController> logger,
        IProductsManagementService productsManagementService)
    {
        _logger = logger;
        _productsManagementService = productsManagementService;
    }

    [HttpPost]
    public IActionResult Post(ProductCreateVm model)
    {
        var createdProductId = _productsManagementService.CreateProduct(model);

        return CreatedAtAction(
            nameof(Get),
            routeValues: new { productId = createdProductId },
            value: new { ProductId = createdProductId });
    }

    [HttpGet("{productId:guid}")]
    public IActionResult Get(Guid productId)
    {
        var product = _productsManagementService.GetProduct(productId);

        return Ok(product);
    }

    [HttpPut("{productId:guid}")]
    public IActionResult Put(Guid productId, ProductUpdateVm model)
    {
        _productsManagementService.UpdateProduct(productId, model);

        return Accepted();
    }

    [HttpDelete("{productId:guid}")]
    public IActionResult Delete(Guid productId)
    {
        _productsManagementService.DeleteProduct(productId);
        
        return Accepted();
    }

    [HttpPost("Search")]
    public IActionResult Search(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        SearchEngineFilter? filter = null, int page = 1, int pageSize = 25)
    {
        var result = _productsManagementService.SearchProducts(filter, page, pageSize);

        return Ok(result);
    }
}
