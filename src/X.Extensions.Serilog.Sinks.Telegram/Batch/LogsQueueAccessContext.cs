using System.Threading.Channels;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch;

public class LogsQueueAccessContext(
    ChannelReader<LogEvent> logsChannelReader) : ILogsQueueAccessor
{
    public Task<List<LogEvent>> DequeueSeveralAsync(int amount)
    {
        var logs = new List<LogEvent>();

        while (amount-- > 0 && logsChannelReader.Count > 0)
        {
            var isDequeued = logsChannelReader.TryRead(out var log);
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
        return logsChannelReader.Count;
    }
}