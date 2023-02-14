using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Abstraction.Services;
using WebApplication1.Common.SearchEngine.Models;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Implementation.ViewModels.Order;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IOrdersManagementService _ordersManagementService;
    private readonly IMapper _mapper;

    public OrdersController(
        ILogger<OrdersController> logger,
        IOrdersRepository ordersRepository,
        IOrdersManagementService ordersManagementService,
        IMapper mapper)
    {
        _logger = logger;
        _ordersRepository = ordersRepository;
        _ordersManagementService = ordersManagementService;
        _mapper = mapper;
    }
    
    [HttpPost]
    public IActionResult Post(OrderCreateVm model)
    {
        var order = _ordersManagementService.CreateOrder(model);
        
        return CreatedAtAction(
            nameof(Get),
            new { Id = order.Id },
            _mapper.Map<OrderVm>(order));
    }
    
    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var order = _ordersManagementService.GetOrder(id);

        return Ok(_mapper.Map<OrderVm>(order));
    }
    
    [HttpPut("{id:guid}/UpdateOrderState")]
    public IActionResult Put(Guid id, UpdateOrderStateVm model)
    {
        var order = _ordersManagementService.UpdateOrderState(id, model);

        return AcceptedAtAction(
            nameof(Get),
            new { Id = id },
            _mapper.Map<OrderVm>(order));
    }
    
    [HttpPut("{id:guid}/UpdateOrderPosition")]
    public IActionResult Put(Guid id, UpdateOrderPositionVm model)
    {
        var order = _ordersManagementService.UpdateOrderPosition(id, model);

        return AcceptedAtAction(
            nameof(Get),
            new { Id = id },
            _mapper.Map<OrderVm>(order));
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _ordersManagementService.DeleteOrder(id);

        return Accepted();
    }

    [HttpGet]
    public IActionResult Get(SearchEngineFilter? filter, int page = 1, int pageSize = 25)
    {
        var orders = _ordersRepository
            .SearchWithPagination(filter, page, pageSize)
            .MapTo<OrderVm>();

        return Ok(orders);
    }
}
