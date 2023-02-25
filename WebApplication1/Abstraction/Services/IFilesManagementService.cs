using WebApplication1.Abstraction.Models;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;

namespace WebApplication1.Abstraction.Services;

public interface IFilesManagementService
{
    Guid SaveFile(IFormFile file);
    FileData GetFileData(Guid id);
    void DeleteFile(Guid id);
    PagedModel<FileData> SearchWithPagination(SearchEngineFilter? filter, int page, int pageSize);
}
