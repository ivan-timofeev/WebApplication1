using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.ViewModels.Customer;

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
        customer = _customersRepository.Create(customer);
        
        return CreatedAtAction(
            nameof(Get), 
            new { id = customer.Id },
            _mapper.Map<CustomerVm>(customer));
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
        customer = _customersRepository.Update(id, customer);
        
        return AcceptedAtAction(
            nameof(Get), 
            new { id = customer.Id },
            _mapper.Map<CustomerVm>(customer));
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _customersRepository.Delete(id);
        
        return Accepted();
    }

    [HttpGet]
    public IActionResult Get(string? searchQuery, int page = 1, int pageSize = 25)
    {
        var result = _customersRepository
            .SearchWithPagination(searchQuery, page, pageSize)
            .MapTo<CustomerVm>();

        return Ok(result);
    }
}
