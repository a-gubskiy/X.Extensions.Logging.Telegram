namespace X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;

public interface ILogsQueueAccessor
{
    public Task<List<LogEvent>> DequeueSeveralAsync(int amount);
    public int GetSize();
}