namespace X.Serilog.Sinks.Telegram;

public class LogEntry
{
    public LogEventLevel Level { get; init; }

    public DateTime UtcTimeStamp { get; init; }

    public MessageTemplate? MessageTemplate { get; init; }

    public string? RenderedMessage { get; init; }

    public string? Properties { get; init; }

    public string? Exception { get; init; }

    public static LogEntry MapFrom(LogEvent logEvent)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        return new LogEntry
        {
            MessageTemplate = logEvent.MessageTemplate,
            RenderedMessage = logEvent.RenderMessage(),
            Level = logEvent.Level,
            UtcTimeStamp = logEvent.Timestamp.ToUniversalTime().UtcDateTime,
            Exception = logEvent.Exception?.ToString(),
            Properties = JsonConvert.SerializeObject(logEvent.Properties, Formatting.Indented)
        };
    }
}