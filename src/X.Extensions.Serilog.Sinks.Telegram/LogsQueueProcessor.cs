using System.Collections.Immutable;
using X.Extensions.Logging.Telegram.Base;
using X.Extensions.Logging.Telegram.Base.Formatters;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Extensions;

namespace X.Extensions.Serilog.Sinks.Telegram;

internal class LogsQueueProcessor
{
    private readonly ILogsQueueAccessor _logsQueueAccessor;

    private readonly ILogFormatter _messageFormatter;

    private readonly TelegramSinkConfiguration _sinkConfiguration;

    public LogsQueueProcessor(
        ILogsQueueAccessor logsQueueAccessor,
        ILogFormatter messageFormatter,
        TelegramSinkConfiguration sinkConfiguration)
    {
        _logsQueueAccessor = logsQueueAccessor;
        _messageFormatter = messageFormatter;
        _sinkConfiguration = sinkConfiguration;
    }

    internal async Task<IImmutableList<string>> GetMessagesFromQueueAsync(int amount)
    {
        var logsBatch = await _logsQueueAccessor.DequeueSeveralAsync(amount);
        var events = logsBatch
            .Select(o => MapLogEntry(o))
            .ToList();

        if (events.Count == 0)
        {
            return ImmutableArray<string>.Empty;
        }

        return _messageFormatter
            .Format(events, _sinkConfiguration.FormatterConfiguration)
            .ToImmutableList();
    }

    private LogEntry MapLogEntry(LogEvent e)
    {
        return new LogEntry
        {
            Message = e.RenderMessage(),
            Level = e.Level.ToTelegramLogLevel(),
            Properties = e.Properties.ToDictionary(k => k.Key, v => v.Value.ToString()),
            UtcTimeStamp = e.Timestamp.UtcDateTime,
            Exception = e.Exception?.ToString()
        };
    }
}