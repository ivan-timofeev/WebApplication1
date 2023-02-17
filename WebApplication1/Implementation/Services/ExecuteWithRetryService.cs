namespace WebApplication1.Services;

public class ExecuteWithRetryService
{
    public ExecutionHistory ExecuteWithRetry(Action action)
    {
        var executionHistory = new ExecutionHistory();
        
        for (var i = 0; i < 5; i++)
        {
            var startedAt = DateTime.UtcNow;
            
            try
            {
                action.Invoke();
                
                executionHistory.Items.Add(new ExecutionHistoryItem
                {
                    AttemptNumber = i + 1,
                    Result = ExecutionResultEnum.Success,
                    ExecutionStartDateTime = startedAt,
                    ExecutionEndDateTime = DateTime.UtcNow
                });

                return executionHistory;
            }
            catch (Exception x)
            {
                executionHistory.Items.Add(new ExecutionHistoryItem
                {
                    AttemptNumber = i + 1,
                    Result = ExecutionResultEnum.Error,
                    Exception = x,
                    ExecutionStartDateTime = startedAt,
                    ExecutionEndDateTime = DateTime.UtcNow
                });
            }

            // 20ms, 70ms, 200ms, ...
            var sleepTime = ((int) Math.Pow(Math.E, i + 1)) * 10;
            Thread.Sleep(sleepTime);
        }

        return executionHistory;
    }

    public class ExecutionHistory
    {
        public int AttemptsCount
            => Items.Max(x => x.AttemptNumber);

        public List<ExecutionHistoryItem> Items { get; set; }
            = new List<ExecutionHistoryItem>();
    }
    
    public class ExecutionHistoryItem
    {
        public int AttemptNumber { get; set; }
        public ExecutionResultEnum Result { get; set; }
        public DateTime ExecutionStartDateTime { get; set; }
        public DateTime ExecutionEndDateTime { get; set; }
        public Exception? Exception { get; set; }
    }
    
    public enum ExecutionResultEnum
    {
        Success,
        Error
    }
}
