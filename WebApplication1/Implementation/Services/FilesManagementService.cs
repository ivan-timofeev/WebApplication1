using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Models;
using WebApplication1.Abstraction.Services;
using WebApplication1.Abstraction.Services.SearchEngine;
using WebApplication1.Common.Exceptions;
using WebApplication1.Data;
using WebApplication1.Helpers.Extensions;
using WebApplication1.Implementation.Helpers.Extensions;
using WebApplication1.Models;
using WebApplication1.Services.SearchEngine.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services;

public class FilesManagementService : IFilesManagementService
{
    private readonly WebApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ISearchEngine _searchEngine;

    public FilesManagementService(
        WebApplicationDbContext dbContext,
        IMapper mapper,
        ISearchEngine searchEngine)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _searchEngine = searchEngine;
    }

    public Guid SaveFile(IFormFile file)
    {
        var fileExtension = new FileInfo(file.FileName).Extension;
        if (!IsTrustedExtension(fileExtension))
        {
            var errorVm = new ErrorVmBuilder()
                .WithGlobalError("The type of the submitted file is not trusted.")
                .WithInfo("ProvidedFileType", fileExtension)
                .WithInfo("TrustedFileTypes", ".png .jpg .jpeg .gif")
                .Build();

            throw new BusinessErrorException("The type of the submitted file is not trusted.",
                StatusCodes.Status400BadRequest, errorVm);
        }

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

    public FileDataVm GetFileData(Guid fileDataId)
    {
        var fileData = _dbContext.FilesData
            .FirstOrDefault(x => x.Id == fileDataId)
            .ThrowIfNotFound(fileDataId);

        var fileDataVm = _mapper.Map<FileDataVm>(fileData);

        return fileDataVm;
    }

    public void DeleteFile(Guid fileDataId)
    {
        var fileData = _dbContext.FilesData
            .FirstOrDefault(x => x.Id == fileDataId)
            .ThrowIfNotFound(fileDataId);

        if (File.Exists(Path.Combine("files", fileData.FileName)))
            File.Delete(Path.Combine("files", fileData.FileName));

        _dbContext.FilesData.Remove(fileData);
        _dbContext.SaveChanges();
    }

    public PagedModel<FileDataVm> SearchWithPagination(SearchEngineFilter? filter, int page, int pageSize)
    {
        var source = _searchEngine
            .ExecuteEngine(_dbContext.FilesData, filter)
            .AsNoTracking();

        var pagedModel = PagedModel<FileData>.Paginate(source, page, pageSize);
        var mappedPagedModel = pagedModel.MapTo<FileDataVm>();

        return mappedPagedModel;
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
