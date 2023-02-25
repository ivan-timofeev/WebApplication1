using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApplication1.Abstraction.Services;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels.Manufacturer;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly IFilesManagementService _filesManagementService;

    public FilesController(
        ILogger<CustomersController> logger,
        IFilesManagementService filesManagementService)
    {
        _logger = logger;
        _filesManagementService = filesManagementService;
    }

    [HttpPost]
    public IActionResult Post(IFormFile file)
    {
        var fileId = _filesManagementService.SaveFile(file);
        var fileData = _filesManagementService.GetFileData(fileId);
        
        return Ok(fileData);
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var fileData = _filesManagementService.GetFileData(id);
        return Ok(fileData);
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _filesManagementService.DeleteFile(id);
        return Accepted();
    }

    [HttpPost("Search")]
    public IActionResult Search(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        SearchEngineFilter? filter = null, int page = 1, int pageSize = 25)
    {
        var result = _filesManagementService
            .SearchWithPagination(filter, page, pageSize);

        return Ok(result);
    }
}
