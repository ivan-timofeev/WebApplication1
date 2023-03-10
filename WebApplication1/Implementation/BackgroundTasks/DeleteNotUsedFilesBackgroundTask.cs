using System.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstraction.Services;
using WebApplication1.Data;

namespace WebApplication1.BackgroundTasks;

public class DeleteNotUsedFilesBackgroundTask : BackgroundTask
{
    private readonly ILogger<DeleteNotUsedFilesBackgroundTask> _logger;
    private readonly IServiceProvider _serviceProvider;
 
    public DeleteNotUsedFilesBackgroundTask(
        ILogger<DeleteNotUsedFilesBackgroundTask> logger,
        IServiceProvider serviceProvider)
        : base(logger, period: TimeSpan.FromSeconds(5))
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override void ExecuteTaskLogic()
    {
        using var scope = _serviceProvider.CreateScope();
        var filesManagementService = scope.ServiceProvider.GetRequiredService<IFilesManagementService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<WebApplicationDbContext>();

        var isTransactionCommitted = false;
        dbContext.Database.BeginTransaction(IsolationLevel.ReadUncommitted);

        try
        {
            // TODO
        }
        finally
        {
            if (!isTransactionCommitted)
            {
                dbContext.Database.RollbackTransaction();
            }
        }
    }
}
