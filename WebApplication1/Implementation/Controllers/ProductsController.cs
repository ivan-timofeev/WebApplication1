using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Models;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
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
        product = _productsRepository.Create(product);
        
        return CreatedAtAction(
            nameof(Get), 
            new { id = product.Id },
            _mapper.Map<ProductVm>(product));
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
        product = _productsRepository.Update(id, product);
        
        return AcceptedAtAction(
            nameof(Get), 
            new { id = product.Id },
            _mapper.Map<ProductVm>(product));
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _productsRepository.Delete(id);
        
        return Accepted();
    }

    [HttpGet]
    public IActionResult Get(string? searchQuery, int page = 1, int pageSize = 25)
    {
        var result = _productsRepository
            .SearchWithPagination(searchQuery, page, pageSize)
            .MapTo<ProductVm>();

        return Ok(result);
    }
}
