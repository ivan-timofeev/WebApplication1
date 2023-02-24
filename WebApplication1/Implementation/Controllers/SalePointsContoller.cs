using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalePointsController : ControllerBase
{
    private readonly ILogger<SalePointsController> _logger;
    private readonly ISalePointsRepository _salePointsRepository;
    private readonly IMapper _mapper;

    public SalePointsController(
        ILogger<SalePointsController> logger,
        ISalePointsRepository salePointsRepository,
        IMapper mapper)
    {
        _logger = logger;
        _salePointsRepository = salePointsRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Post(SalePointCreateVm model)
    {
        var salePoint = _mapper.Map<SalePoint>(model);
        var createdSalePointId = _salePointsRepository.Create(salePoint);
        
        return CreatedAtAction(
            nameof(Get), 
            value: new { SalePointId = createdSalePointId});
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var result = _salePointsRepository
            .Read(id);

        return Ok(_mapper.Map<SalePointVm>(result));
    }

    [HttpPut("{id:guid}")]
    public IActionResult Put(Guid id, SalePointUpdateVm model)
    {
        var salePoint = _mapper.Map<SalePoint>(model);
        _salePointsRepository.Update(id, salePoint);
        
        return Accepted();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _salePointsRepository.Delete(id);
        
        return Accepted();
    }

    [HttpPost("Search")]
    public IActionResult Search(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        SearchEngineFilter? filter = null, int page = 1, int pageSize = 25)
    {
        var result = _salePointsRepository
            .SearchWithPagination(filter, page, pageSize)
            .MapTo<SalePointVm>();

        return Ok(result);
    }
}
