namespace X.Serilog.Sinks.Telegram;

public class LogEntry
{
    public LogEventLevel Level { get; set; }

    public DateTime UtcTimeStamp { get; set; }

    public MessageTemplate MessageTemplate { get; set; }

    public string RenderedMessage { get; set; }

    public string Properties { get; set; }

    public string Exception { get; set; }

    public static LogEntry MapFrom(LogEvent logEvent)
    {
        if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));

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