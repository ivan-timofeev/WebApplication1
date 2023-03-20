using WebApplication1.Abstractions.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Abstractions.Services;

public interface IFilesManagementService
{
    Guid SaveFile(IFormFile file);
    FileDataVm GetFileData(Guid id);
    void DeleteFile(Guid id);
    PagedModel<FileDataVm> SearchWithPagination(SearchEngineFilter? filter, int page, int pageSize);
}
