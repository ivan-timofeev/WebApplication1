using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.Common.Extensions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductsRepository _productsRepository;
    private readonly IMapper _mapper;

    public ProductsController(
        ILogger<ProductsController> logger,
        IProductsRepository productsRepository,
        IMapper mapper)
    {
        _logger = logger;
        _productsRepository = productsRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Post(ProductCreateVm model)
    {
        var product = _mapper.Map<Product>(model);
        var createdProductId = _productsRepository.Create(product);

        return CreatedAtAction(
            nameof(Get),
            routeValues: new { id = createdProductId },
            value: new { ProductId = createdProductId });
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var result = _productsRepository
            .Read(id);

        return Ok(_mapper.Map<ProductVm>(result));
    }

    [HttpPut("{id:guid}")]
    public IActionResult Put(Guid id, ProductUpdateVm model)
    {
        var product = _mapper.Map<Product>(model);
        _productsRepository.Update(id, product);
        
        return Accepted();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _productsRepository.Delete(id);
        
        return Accepted();
    }

    [HttpPost("Search")]
    public IActionResult Search(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        SearchEngineFilter? filter = null, int page = 1, int pageSize = 25)
    {
        var result = _productsRepository
            .SearchWithPagination(filter, page, pageSize)
            .MapTo<ProductVm>();

        return Ok(result);
    }
}
