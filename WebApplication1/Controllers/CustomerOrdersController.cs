using Microsoft.AspNetCore.Mvc;
using WebApplication1.Abstractions.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/Customers/{customerId:guid}/Orders")]
public class CustomerOrdersController : ControllerBase
{
    private readonly ICustomersManagementService _customersManagementService;

    public CustomerOrdersController(ICustomersManagementService customersManagementService)
    {
        _customersManagementService = customersManagementService;
    }

    [HttpGet]
    public IActionResult Get(Guid customerId)
    {
        var result = _customersManagementService.GetCustomerOrders(customerId);

        return Ok(result);
    }
}
