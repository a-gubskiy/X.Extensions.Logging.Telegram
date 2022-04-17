using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Formatters;

public class DefaultNotificationFormatter : MessageFormatterBase
{
    public override string Format(
        LogEntry logEntry,
        FormatterConfiguration config,
        Func<LogEntry, FormatterConfiguration, string> formatter = null)
    {
        if (logEntry is null) throw new ArgumentNullException(nameof(logEntry));

        formatter ??= DefaultFormatter;

        var message = formatter(logEntry, config);
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException(nameof(message));
        }

        return message;

    }

    private string DefaultFormatter(LogEntry logEntry, FormatterConfiguration config)
    {
        if (logEntry is null) throw new ArgumentNullException(nameof(logEntry));

        var level = config.UseEmoji ? ToEmoji(logEntry.Level) : ToString(logEntry.Level);

        var sb = new StringBuilder();

        sb.Append("<em>[").AppendFormat("{0:G}", logEntry.UtcTimeStamp).Append(' ').Append(level).Append("]</em>").Append(' ').Append(config.ReadableApplicationName);

        sb.AppendLine();
        sb.AppendLine();

        if (NotEmpty(logEntry.RenderedMessage))
        {
            sb.Append("<em>").Append("Message: ").Append("</em>").Append("<code>").Append(logEntry.RenderedMessage).Append("</code>").AppendLine();
        }

        return sb.ToString();
    }
}