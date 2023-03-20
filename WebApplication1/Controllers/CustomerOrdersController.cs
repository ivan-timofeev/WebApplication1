using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Abstractions.Data.Repositories;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/Customers/{customerId:guid}/Orders")]
public class CustomerOrdersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly ICustomerOrdersRepository _customerOrdersRepository;
    private readonly ICustomersRepository _customersRepository;
    private readonly IMapper _mapper;

    public CustomerOrdersController(
        ILogger<CustomersController> logger,
        ICustomerOrdersRepository customerOrdersRepository,
        ICustomersRepository customersRepository,
        IMapper mapper)
    {
        _logger = logger;
        _customerOrdersRepository = customerOrdersRepository;
        _customersRepository = customersRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get(Guid customerId)
    {
        var customer = _customersRepository.Read(customerId);
        var orders = _customerOrdersRepository.GetCustomerOrders(customer);

        var model = new PersonalAreaVm(
            Customer: _mapper.Map<CustomerVm>(customer),
            CustomerOrders: _mapper.Map<CustomerOrderVm[]>(orders));

        return Ok(model);
    }
}
