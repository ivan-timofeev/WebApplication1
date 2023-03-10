namespace WebApplication1.BackgroundTasks;

public abstract class BackgroundTask : IHostedService, IDisposable
{
    private readonly ILogger _logger;
    private readonly Timer? _timer;
    private bool _isTaskEnabled;

    protected BackgroundTask(
        ILogger logger,
        TimeSpan period)
    {
        _logger = logger;
        _isTaskEnabled = false;

        _timer = new Timer(o => {
                if (_isTaskEnabled)
                    ExecuteLogic();
            },
            state: null,
            dueTime: TimeSpan.Zero,
            period: period);
    }

    public void Dispose()
    {
        try
        {
            _timer?.Dispose();
        }
        catch
        {
            // Ignored
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _isTaskEnabled = true;
        ExecuteLogic();

        await Task.CompletedTask;
    }
 
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _isTaskEnabled = false;
        await Task.CompletedTask;
    }

    private void ExecuteLogic()
    {
        try
        {
            ExecuteTaskLogic();
        }
        catch (Exception x)
        {
            _logger.LogCritical(message: 
                "Execution fails.\n" +
                "Next start in {d}\n" +
                "Exception: {x}", DateTime.UtcNow.AddHours(1), x);
        }
    }

    protected abstract void ExecuteTaskLogic();
}
