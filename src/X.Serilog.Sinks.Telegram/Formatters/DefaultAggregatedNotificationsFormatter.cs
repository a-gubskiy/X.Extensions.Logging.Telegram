using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Formatters;

public class DefaultAggregatedNotificationsFormatter : MessageFormatterBase
{
    public override string Format(
        IEnumerable<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<IEnumerable<LogEntry>, FormatterConfiguration, string> formatter = null)
    {
        if (logEntries is null) throw new ArgumentNullException(nameof(logEntries));

        formatter ??= DefaultFormatter;

        var message = formatter(logEntries, config);
        if (!NotEmpty(message))
        {
            throw new ArgumentException(nameof(message));
        }

        return message;
    }

    private string DefaultFormatter(IEnumerable<LogEntry> logEntries, FormatterConfiguration config)
    {
        var entries = logEntries as LogEntry[] ?? logEntries.ToArray();

        var sb = new StringBuilder();
        logEntries = entries.OrderBy(x => x.UtcTimeStamp);

        var beginTs = logEntries.First().UtcTimeStamp;
        var endTs = logEntries.Last().UtcTimeStamp;

        sb.Append("<em>[").Append($"{beginTs:G}").Append('—').Append($"{endTs:G}").Append("]</em>").Append(' ').Append(config.ReadableApplicationName)
            .AppendLine()
            .AppendLine();

        foreach (var logEntry in logEntries)
        {
            if (!NotEmpty(logEntry)) continue;

            var level = config.UseEmoji ? ToEmoji(logEntry.Level) : ToString(logEntry.Level);

            sb.Append("<em>[").Append($"{logEntry.UtcTimeStamp:T}").Append(' ').Append(level).Append("]</em>");

            if (NotEmpty(logEntry.RenderedMessage))
            {
                sb.Append(" <code>").Append(logEntry.RenderedMessage).Append("</code>;")
                    .AppendLine();
            }
        }

        return sb.ToString();
    }
}