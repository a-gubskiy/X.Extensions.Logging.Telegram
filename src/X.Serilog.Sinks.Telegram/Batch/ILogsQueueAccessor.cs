namespace X.Serilog.Sinks.Telegram.Batch;

public interface ILogsQueueAccessor
{
    public Task<List<LogEvent>> DequeueSeveralAsync(int amount);
    public int GetSize();
}