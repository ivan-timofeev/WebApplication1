using WebApplication1.Abstraction.Services;
using WebApplication1.Data;
using WebApplication1.Implementation.ViewModels.Order;
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
            TimeSpan.FromMinutes(1));
 
        return Task.CompletedTask;
    }
 
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void ExecuteLogic()
    {
        using var scope = _serviceProvider.CreateScope();
        var ordersManagementService = scope.ServiceProvider.GetRequiredService<IOrdersManagementService>();
        var dbContext = scope.ServiceProvider.GetRequiredService<WebApplicationDbContext>();
            
        var expiredOrderIds = dbContext.Orders
            .Where(x => x.OrderStateHierarchical.Count == 1
                        && x.OrderStateHierarchical.First().State == OrderStateEnum.Creating)
            .Where(x => DateTime.UtcNow - x.OrderStateHierarchical.First().EnteredDateTimeUtc
                        >= TimeSpan.FromMinutes(15))
            .Select(x => x.Id)
            .ToArray();

        foreach (var expiredOrderId in expiredOrderIds)
        {
            var updateOrderStateVm = new UpdateOrderStateVm(
                NewOrderState: OrderStateEnum.Canceled,
                EnterDescription: "Заказ отменен системой",
                Details: "Время резервации вышло");
            
            ordersManagementService.UpdateOrderState(expiredOrderId, updateOrderStateVm);
            
            _logger.LogInformation("Резервация отменена: {expiredOrderId}", expiredOrderId);
        }
    }
}
