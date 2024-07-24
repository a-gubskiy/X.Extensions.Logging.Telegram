namespace X.Serilog.Sinks.Telegram;

public class LogEntry
{
    private  LogEntry()
    {
    }

    public LogEventLevel Level { get; private init; }

    public DateTime UtcTimeStamp { get; private init; }

    public string? RenderedMessage { get; private init; }

    public Dictionary<string, string>? Properties { get; private init; }

    public string? Exception { get; private init; }

    public static LogEntry MapFrom(LogEvent logEvent)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        return new LogEntry
        {
            RenderedMessage = logEvent.RenderMessage(),
            Level = logEvent.Level,
            UtcTimeStamp = logEvent.Timestamp.ToUniversalTime().UtcDateTime,
            Exception = logEvent.Exception?.ToString(),
            Properties = logEvent.Properties.ToDictionary(x => x.Key, x => x.Value.ToString())
        };
    }
}