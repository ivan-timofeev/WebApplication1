using WebApplication1.Abstraction.Data.Repositories;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Implementation.BackgroundTasks;

public class CheckExpiredOrdersBackgroundTask : IHostedService, IDisposable
{
    private readonly ILogger<CheckExpiredOrdersBackgroundTask> _logger;
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;
 
    public CheckExpiredOrdersBackgroundTask(
        ILogger<CheckExpiredOrdersBackgroundTask> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
 
    public void Dispose()
    {
        _timer?.Dispose();
    }
 
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(o => {
                ExecuteLogic();
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));
 
        return Task.CompletedTask;
    }
 
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void ExecuteLogic()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WebApplicationDbContext>();
            
        var expiredOrders = dbContext.Orders
            .Where(x => x.OrderStateHierarchical.Count == 1
                        && x.OrderStateHierarchical.First().State == OrderStateEnum.Creating)
            .Where(x => DateTime.UtcNow - x.OrderStateHierarchical.First().EnteredDateTimeUtc
                        >= TimeSpan.FromSeconds(10))
            .Select(x => x.Id)
            .ToArray();
        
        _logger.LogInformation("Просрочек: " + expiredOrders.Length);
    }
}