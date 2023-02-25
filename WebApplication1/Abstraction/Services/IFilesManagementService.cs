using WebApplication1.Models;

namespace WebApplication1.Abstraction.Services;

public interface IFilesManagementService
{
    Guid SaveFile(IFormFile file);
    FileData GetFileData(Guid id);
    void DeleteFile(Guid id);
}
