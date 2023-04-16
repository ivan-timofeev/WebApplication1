using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Abstractions.Services;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalePointsController : ControllerBase
{
    private readonly ILogger<SalePointsController> _logger;
    private readonly ISalePointsManagementService _salePointsManagementService;

    public SalePointsController(
        ILogger<SalePointsController> logger,
        ISalePointsManagementService salePointsManagementService )
    {
        _logger = logger;
        _salePointsManagementService = salePointsManagementService;
    }

    [HttpPost]
    public IActionResult Post(SalePointCreateVm model)
    {
        var createdSalePointId = _salePointsManagementService.CreateSalePoint(model);
        
        return CreatedAtAction(
            nameof(Get),
            routeValues: new { salePointId = createdSalePointId },
            value: new { SalePointId = createdSalePointId});
    }

    [HttpGet("{salePointId:guid}")]
    public IActionResult Get(Guid salePointId)
    {
        var salePoint = _salePointsManagementService.GetSalePoint(salePointId);

        return Ok(salePoint);
    }

    [HttpPut("{id:guid}")]
    public IActionResult Put(Guid id, SalePointUpdateVm model)
    {
        _salePointsManagementService.UpdateSalePoint(id, model);
        
        return Accepted();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _salePointsManagementService.DeleteSalePoint(id);
        
        return Accepted();
    }

    [HttpPost("Search")]
    public IActionResult Search(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        SearchEngineFilter? filter = null, int page = 1, int pageSize = 25)
    {
        var result = _salePointsManagementService.SearchSalePoints(filter, page, pageSize);

        return Ok(result);
    }
}
