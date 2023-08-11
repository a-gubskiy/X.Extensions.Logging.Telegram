using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Formatters;

public class DefaultAggregatedNotificationsFormatter : MessageFormatterBase
{
    public override List<string> Format(
        ICollection<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<ICollection<LogEntry>, FormatterConfiguration, List<string>> formatter = null)
    {
        formatter ??= DefaultFormatter;
        return base.Format(logEntries, config, formatter);
    }

    private List<string> DefaultFormatter(IEnumerable<LogEntry> logEntries, FormatterConfiguration config)
    {
        var sb = new StringBuilder();

        logEntries = logEntries.OrderBy(x => x.UtcTimeStamp).ToList();

        var batchBeginTimestamp = logEntries.First().UtcTimeStamp;
        var batchEndTimestamp = logEntries.Last().UtcTimeStamp;

        if (config.TimeZone is not null)
        {
            batchBeginTimestamp = TimeZoneInfo.ConvertTime(batchBeginTimestamp, config.TimeZone);
            batchEndTimestamp = TimeZoneInfo.ConvertTime(batchBeginTimestamp, config.TimeZone);
        }

        sb.Append("<em>[").Append($"{batchBeginTimestamp:G}").Append('—').Append($"{batchEndTimestamp:G}")
            .Append("]</em>").Append(' ')
            .Append(config.ReadableApplicationName)
            .AppendLine()
            .AppendLine();

        foreach (var logEntry in logEntries)
        {
            if (!NotEmpty(logEntry)) continue;

            var level = config.UseEmoji ? ToEmoji(logEntry.Level) : ToString(logEntry.Level);

            sb.Append(level).Append(' ').Append("<em>[").Append($"{logEntry.UtcTimeStamp:T}").Append("]</em>");

            if (NotEmpty(logEntry.RenderedMessage))
            {
                sb.Append(" <code>").Append(logEntry.RenderedMessage).Append("</code>;")
                    .AppendLine();
            }
        }

        return new List<string> { sb.ToString() };
    }
}