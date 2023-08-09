using System.Threading.Channels;

namespace X.Serilog.Sinks.Telegram.Batch;

public class LogsQueueAccessContext : ILogsQueueAccessor
{
    private readonly ChannelReader<LogEvent> _logsChannelReader;

    public LogsQueueAccessContext(ChannelReader<LogEvent> logsChannelReader)
    {
        _logsChannelReader = logsChannelReader;
    }

    public Task<List<LogEvent>> DequeueSeveralAsync(int amount)
    {
        var logs = new List<LogEvent>();

        while (amount-- > 0 && _logsChannelReader.Count > 0)
        {
            var isDequeued = _logsChannelReader.TryRead(out var log);
            if (!isDequeued || log is null)
            {
                break;
            }

            logs.Add(log);
        }

        return Task.FromResult(logs);
    }

    public int GetSize()
    {
        return _logsChannelReader.Count;
    }
}

public interface ILogsQueueAccessor
{
    public Task<List<LogEvent>> DequeueSeveralAsync(int amount);
    public int GetSize();
}