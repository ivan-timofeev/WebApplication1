using Microsoft.AspNetCore.Mvc;
using WebApplication1.Abstraction.Services;

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
}
