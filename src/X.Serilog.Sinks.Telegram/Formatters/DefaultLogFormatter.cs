using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Formatters;

internal class DefaultLogFormatter : MessageFormatterBase
{
    /// <inheritdoc cref="MessageFormatterBase"/>
    /// <exception cref="ArgumentNullException">Throws when the log entry is null.</exception>
    /// <exception cref="ArgumentException">Throws when, after using the formatter, the message is null, empty, or whitespace.</exception>
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

        sb.Append("<em>[").Append($"{logEntry.UtcTimeStamp:G}").Append(' ').Append(level).Append("]</em>").Append(' ').Append(config.ReadableApplicationName);

        sb.AppendLine();
        sb.AppendLine();

        if (NotEmpty(logEntry.RenderedMessage))
        {
            sb.Append("<em>").Append("Message: ").Append("</em>").Append("<code>").Append(logEntry.RenderedMessage).Append("</code>").AppendLine();
        }

        if (NotEmpty(logEntry.Exception))
        {
            sb.Append("<em>").Append("Exception: ").Append("</em>").Append("<code>").Append(logEntry.Exception).Append("</code>").AppendLine();
        }

        if (NotEmpty(logEntry.Properties))
        {
            sb.Append("<em>").Append("Properties: ").Append("</em>").AppendLine()
                .Append("<code>").Append(logEntry.Properties).Append("</code>").AppendLine();
        }

        return sb.ToString();
    }
}