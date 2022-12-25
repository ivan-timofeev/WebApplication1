using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Abstraction.Models;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class SalePointsController : ControllerBase
{
    private readonly ILogger<SalePointsController> _logger;
    private readonly IRepository<SalePoint> _salePointsRepository;
    private readonly IMapper _mapper;

    public SalePointsController(
        ILogger<SalePointsController> logger,
        IRepository<SalePoint> salePointsRepository,
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
        salePoint = _salePointsRepository.Create(salePoint);
        
        return CreatedAtAction(
            nameof(Get), 
            new { id = salePoint.Id },
            _mapper.Map<SalePointVm>(salePoint));
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
        salePoint = _salePointsRepository.Update(id, salePoint);
        
        return AcceptedAtAction(
            nameof(Get), 
            new { id = salePoint.Id },
            _mapper.Map<SalePointVm>(salePoint));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _salePointsRepository.Delete(id);
        
        return Accepted();
    }

    [HttpGet]
    public IActionResult Get(string? searchQuery, int page = 1, int pageSize = 25)
    {
        var result = _salePointsRepository
            .SearchWithPagination(searchQuery, page, pageSize)
            .MapTo<SalePointVm>();

        return Ok(result);
    }
}
