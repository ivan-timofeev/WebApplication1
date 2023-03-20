using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstractions.Data;
using WebApplication1.Abstractions.Services;

namespace WebApplication1.BackgroundTasks;

public class DeleteNotUsedFilesBackgroundTask : BackgroundTask
{
    private readonly ILogger<DeleteNotUsedFilesBackgroundTask> _logger;
    private readonly IServiceProvider _serviceProvider;
 
    public DeleteNotUsedFilesBackgroundTask(
        ILogger<DeleteNotUsedFilesBackgroundTask> logger,
        IServiceProvider serviceProvider)
        : base(logger, period: TimeSpan.FromHours(1))
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override void ExecuteTaskLogic()
    {
        using var scope = _serviceProvider.CreateScope();
        var filesManagementService = scope.ServiceProvider.GetRequiredService<IFilesManagementService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<WebApplicationDbContext>();

        var hourAgoDateTime = DateTime.UtcNow - TimeSpan.FromHours(1);

        var productImages = dbContext.Products
            .Where(x => x.ImageUri != null)
            .Select(x => x.ImageUri);
        var manufacturerImages = dbContext.Manufacturers
            .Where(x => x.ImageUri != null)
            .Select(x => x.ImageUri);
        var usedImages = productImages
            .Union(manufacturerImages);

        var notUsedFileIds = dbContext.FilesData
            .AsNoTracking()
            .Where(x => x.CreatedDateTimeUtc <= hourAgoDateTime)
            .Where(x => !usedImages.Contains(x.FileUri))
            .Select(x => x.Id)
            .Take(1000)
            .ToArray();

        foreach (var notUsedFileId in notUsedFileIds)
        {
            try
            {
                filesManagementService.DeleteFile(notUsedFileId);
            }
            catch
            {
                // Ignored
            }
        }
    }
}
