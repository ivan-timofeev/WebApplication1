using WebApplication1.Abstraction.Services;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class FilesManagementService : IFilesManagementService
{
    private readonly WebApplicationDbContext _dbContext;

    public FilesManagementService(WebApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Guid SaveFile(IFormFile file)
    {
        var fileExtension = new FileInfo(file.FileName).Extension;
        if (!IsTrustedExtension(fileExtension))
            throw new Exception("NOT TRUSTED FILE TYPE");

        var fileData = new FileData();
        _dbContext.FilesData.Add(fileData);
        _dbContext.SaveChanges();

        fileData.FileName = fileData.Id + fileExtension;
        fileData.FileUri = "files/" + fileData.FileName;
        _dbContext.SaveChanges();

        try
        {
            if (!Directory.Exists("files"))
                Directory.CreateDirectory("files");
            
            using var stream = new FileStream(Path.Combine("files", fileData.FileName), FileMode.Create);
            file.CopyTo(stream);
        }
        catch
        {
            _dbContext.FilesData.Remove(fileData);
            _dbContext.SaveChanges();
            throw;
        }

        return fileData.Id;
    }

    public FileData GetFileData(Guid id)
    {
        return _dbContext.FilesData.First(x => x.Id == id);
    }

    public void DeleteFile(Guid id)
    {
        var fileData = _dbContext.FilesData.First(x => x.Id == id);
        _dbContext.FilesData.Remove(fileData);
        _dbContext.SaveChanges();
    }

    private bool IsTrustedExtension(string extension)
    {
        return extension switch
        {
            ".png" => true,
            ".jpg" => true,
            ".jpeg" => true,
            ".gif" => true,
            _ => false
        };
    }
}
