using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Services;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.SearchEngine;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrdersManagementService _ordersManagementService;
    private readonly IMapper _mapper;

    public OrdersController(
        ILogger<OrdersController> logger,
        IOrdersRepository ordersRepository,
        IOrdersManagementService ordersManagementService,
        IMapper mapper)
    {
        _logger = logger;
        _ordersManagementService = ordersManagementService;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Post(Guid customerId)
    {
        var createdOrderId = _ordersManagementService.CreateOrder(customerId);
        
        return CreatedAtAction(
            nameof(Get),
            routeValues: new { orderId = createdOrderId },
            value: new { OrderId = createdOrderId });
    }

    [HttpGet("{orderId:guid}")]
    public IActionResult Get(Guid orderId)
    {
        var order = _ordersManagementService.GetOrder(orderId);

        return Ok(_mapper.Map<OrderVm>(order));
    }

    [HttpPut("{id:guid}")]
    public IActionResult Put(Guid id, UpdateOrderStateVm model)
    {
        _ordersManagementService.UpdateOrder(id, model);

        return Accepted();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _ordersManagementService.DeleteOrder(id);

        return Accepted();
    }

    [HttpPost("Search")]
    public IActionResult Search(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        SearchEngineFilterVm? filter = null, int page = 1, int pageSize = 25)
    {
        var pagedModel = _ordersManagementService
            .SearchWithPagination(filter, page, pageSize);

        return Ok(pagedModel);
    }
}
