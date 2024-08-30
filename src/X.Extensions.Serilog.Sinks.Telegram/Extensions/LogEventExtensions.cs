using X.Extensions.Logging.Telegram.Base;

namespace X.Extensions.Serilog.Sinks.Telegram.Extensions;

public static class LogEventExtensions
{
    public static LogEntry ToLogEntry(this LogEvent logEvent)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        return new LogEntry
        {
            Message = logEvent.RenderMessage(),
            Level = logEvent.Level.ToTelegramLogLevel(),
            UtcTimeStamp = logEvent.Timestamp.ToUniversalTime().UtcDateTime,
            Exception = logEvent.Exception?.ToString(),
            Properties = logEvent.Properties.ToDictionary(x => x.Key, x => x.Value.ToString())
        };
    }
}