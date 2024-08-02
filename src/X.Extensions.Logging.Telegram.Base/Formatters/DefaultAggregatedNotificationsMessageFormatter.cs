using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X.Extensions.Logging.Telegram.Base.Configuration;

namespace X.Extensions.Logging.Telegram.Base.Formatters;

public class DefaultAggregatedNotificationsMessageFormatter : MessageFormatterBase
{
    public override List<string> Format(
        ICollection<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<ICollection<LogEntry>, FormatterConfiguration, List<string>>? formatter = null)
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
            batchEndTimestamp = TimeZoneInfo.ConvertTime(batchEndTimestamp, config.TimeZone);
        }

        sb.Append("<b>Logs from ").Append($"{batchBeginTimestamp:G}").Append(" to ")
            .Append($"{batchEndTimestamp:G}").Append("</b>")
            .AppendLine()
            .AppendLine();

        foreach (var logEntry in logEntries)
        {
            var level = config.UseEmoji
                ? LogLevelMarkerRenderer.RenderMarker(logEntry.Level)
                : logEntry.Level.ToString();

            sb.Append(level).Append(' ').Append("<em>[").Append($"{logEntry.UtcTimeStamp:G}").Append("]</em>");

            if (!string.IsNullOrEmpty(config.ReadableApplicationName))
            {
                sb.Append(" <strong>").Append(config.ReadableApplicationName).Append("</strong>");
            }

            if (!string.IsNullOrWhiteSpace(logEntry.Message))
            {
                sb.AppendLine().Append("Message: <code>").Append(logEntry.Message).Append("</code>");
            }

            if (config.IncludeException && !string.IsNullOrWhiteSpace(logEntry.Exception))
            {
                sb.AppendLine().Append("Exception: <code>").Append(logEntry.Exception).Append("</code>");
            }

            if (config.IncludeProperties && logEntry.Properties != null && logEntry.Properties.Any())
            {
                sb.AppendLine().Append("Properties: ");
                foreach (var property in logEntry.Properties)
                {
                    sb.Append("<code>").Append(property.Key).Append(": ").Append(property.Value)
                        .Append("</code>; ");
                }
            }

            sb.AppendLine().AppendLine();
        }

        return [sb.ToString().TrimEnd()];
    }
}