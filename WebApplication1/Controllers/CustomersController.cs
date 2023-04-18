using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Abstractions.Services;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;



[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly ICustomersManagementService _customersManagementService;

    public CustomersController(
        ILogger<CustomersController> logger,
        ICustomersManagementService customersManagementService)
    {
        _logger = logger;
        _customersManagementService = customersManagementService;
    }
    
    [HttpPost]
    public IActionResult Post(CustomerCreateVm model)
    {
        var createdCustomerId = _customersManagementService.CreateCustomer(model);
        
        return CreatedAtAction(
            nameof(Get),
            routeValues: new { customerId = createdCustomerId },
            value: new { CustomerId = createdCustomerId });
    }
    
    [HttpGet("{customerId:guid}")]
    public IActionResult Get(Guid customerId)
    {
        var customer = _customersManagementService.GetCustomer(customerId);

        return Ok(customer);
    }
    
    [HttpPut("{customerId:guid}")]
    public IActionResult Put(Guid customerId, CustomerUpdateVm model)
    {
        _customersManagementService.UpdateCustomer(customerId, model);
        
        return Accepted();
    }
    
    [HttpDelete("{customerId:guid}")]
    public IActionResult Delete(Guid customerId)
    {
        _customersManagementService.DeleteCustomer(customerId);
        
        return Accepted();
    }

    [HttpPost("Search")]
    public IActionResult Search(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        SearchEngineFilter? filter = null,
        int page = 1, int pageSize = 25)
    {
        var customers = _customersManagementService.SearchCustomers(filter, page, pageSize);

        return Ok(customers);
    }
}
