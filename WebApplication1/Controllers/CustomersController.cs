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
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly ICustomersRepository _customersRepository;
    private readonly IMapper _mapper;

    public CustomersController(
        ILogger<CustomersController> logger,
        ICustomersRepository customersRepository,
        IMapper mapper)
    {
        _logger = logger;
        _customersRepository = customersRepository;
        _mapper = mapper;
    }
    
    [HttpPost]
    public IActionResult Post(CustomerCreateVm model)
    {
        var customer = _mapper.Map<Customer>(model);
        var createdCustomerId = _customersRepository.Create(customer);
        
        return CreatedAtAction(
            nameof(Get),
            routeValues: new { id = createdCustomerId },
            value: new { CustomerId = createdCustomerId });
    }
    
    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var result = _customersRepository.Read(id);

        return Ok(_mapper.Map<CustomerVm>(result));
    }
    
    [HttpPut("{id:guid}")]
    public IActionResult Put(Guid id, CustomerUpdateVm model)
    {
        var customer = _mapper.Map<Customer>(model);
        _customersRepository.Update(id, customer);
        
        return Accepted();
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _customersRepository.Delete(id);
        
        return Accepted();
    }

    [HttpPost("Search")]
    public IActionResult Search(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        SearchEngineFilter? filter = null,
        int page = 1, int pageSize = 25)
    {
        var result = _customersRepository
            .SearchWithPagination(filter, page, pageSize)
            .MapTo<CustomerVm>();

        return Ok(result);
    }
}