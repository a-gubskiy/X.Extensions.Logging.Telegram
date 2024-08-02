using System.Collections.Immutable;
using X.Extensions.Logging.Telegram.Base.Formatters;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Extensions;

namespace X.Extensions.Serilog.Sinks.Telegram;

internal class LogsQueueProcessor
{
    private readonly ILogsQueueAccessor _logsQueueAccessor;

    private readonly ITelegramMessageFormatter _telegramMessageFormatter;

    private readonly TelegramSinkConfiguration _sinkConfiguration;

    public LogsQueueProcessor(
        ILogsQueueAccessor logsQueueAccessor,
        ITelegramMessageFormatter telegramMessageFormatter,
        TelegramSinkConfiguration sinkConfiguration)
    {
        _logsQueueAccessor = logsQueueAccessor;
        _telegramMessageFormatter = telegramMessageFormatter;
        _sinkConfiguration = sinkConfiguration;
    }

    internal async Task<IImmutableList<string>> GetMessagesFromQueueAsync(int amount)
    {
        var logsBatch = await _logsQueueAccessor.DequeueSeveralAsync(amount);
        var events = logsBatch
            .Select(o => o.ToLogEntry())
            .ToList();

        if (events.Count == 0)
        {
            return ImmutableArray<string>.Empty;
        }

        return _telegramMessageFormatter
            .Format(events, _sinkConfiguration.FormatterConfiguration)
            .ToImmutableList();
    }
}