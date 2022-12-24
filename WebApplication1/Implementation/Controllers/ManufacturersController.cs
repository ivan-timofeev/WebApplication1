using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Abstraction.Models;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using WebApplication1.ViewModels.Manufacturer;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class ManufacturersController : ControllerBase
{
    private readonly ILogger<ManufacturersController> _logger;
    private readonly IRepository<Manufacturer> _manufacturersRepository;
    private readonly IMapper _mapper;

    public ManufacturersController(
        ILogger<ManufacturersController> logger,
        IRepository<Manufacturer> manufacturersRepository,
        IMapper mapper)
    {
        _logger = logger;
        _manufacturersRepository = manufacturersRepository;
        _mapper = mapper;
    }
    
    [HttpPost]
    public IActionResult Post(ManufacturerCreateVm model)
    {
        var manufacturer = _mapper.Map<Manufacturer>(model);
        manufacturer = _manufacturersRepository.Create(manufacturer);
        
        return CreatedAtAction(
            nameof(Get), 
            new { id = manufacturer.Id },
            _mapper.Map<ManufacturerVm>(manufacturer));
    }
    
    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var result = _manufacturersRepository.Read(id);

        return Ok(_mapper.Map<ManufacturerVm>(result));
    }
    
    [HttpPut("{id:guid}")]
    public IActionResult Put(Guid id, ManufacturerUpdateVm model)
    {
        var manufacturer = _mapper.Map<Manufacturer>(model);
        manufacturer = _manufacturersRepository.Update(id, manufacturer);
        
        return AcceptedAtAction(
            nameof(Get), 
            new { id = manufacturer.Id },
            _mapper.Map<ManufacturerVm>(manufacturer));
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _manufacturersRepository.Delete(id);
        
        return Accepted();
    }

    [HttpGet]
    public IActionResult Get(string? searchQuery, int page = 1, int pageSize = 25)
    {
        var result = _manufacturersRepository
            .SearchWithPagination(searchQuery, page, pageSize)
            .MapTo<ManufacturerVm>();

        return Ok(result);
    }
}
