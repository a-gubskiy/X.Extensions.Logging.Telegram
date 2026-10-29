using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using X.Extensions.Logging.Telegram.Base.Configuration;

namespace X.Extensions.Logging.Telegram.Base.Formatters;

public class DefaultLogFormatter : MessageFormatterBase
{
    /// <inheritdoc cref="MessageFormatterBase"/>
    public override List<string> Format(ICollection<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<ICollection<LogEntry>, FormatterConfiguration, List<string>>? formatter = null)
    {
        if (!NotEmpty(logEntries))
        {
            return Empty;
        }

        formatter ??= DefaultFormatter;

        if (logEntries.Count is 1)
        {
            return base.Format(logEntries, config, formatter);
        }

        var formattedMessages = new List<string>();

        foreach (var logEntry in logEntries)
        {
            formattedMessages.AddRange(base.Format([logEntry], config, formatter));
        }

        return formattedMessages;
    }

    private static List<string> DefaultFormatter(ICollection<LogEntry> logEntries, FormatterConfiguration config)
    {
        var result = new List<string>(logEntries.Count);

        foreach (var logEntry in logEntries)
        {
            result.Add(FormatMessage(logEntry, config));
        }

        return result;
    }

    private static string FormatMessage(LogEntry logEntry, FormatterConfiguration config)
    {
        var sb = new StringBuilder();
        var timeZone = config.TimeZone ?? TimeZoneInfo.Utc;
        var timestamp = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(logEntry.UtcTimeStamp, DateTimeKind.Utc), timeZone);

        var logLevelMarker = config.UseEmoji
            ? LogLevelMarkerRenderer.RenderMarker(logEntry.Level) + logEntry.Level
            : logEntry.Level.ToString();

        sb.Append(logLevelMarker)
            .Append(" [").Append(Encode(config.ReadableApplicationName ?? "YourApp")).Append(']')
            .Append(" [").Append($"{timestamp:yyyy-MM-dd HH:mm:ss}");

        if (timeZone.Equals(TimeZoneInfo.Utc))
        {
            sb.Append(" UTC");
        }

        sb.Append(']').AppendLine();

        if (!string.IsNullOrWhiteSpace(logEntry.Message))
        {
            sb.AppendLine()
                .Append("<b>Message:</b> <code>").Append(Encode(logEntry.Message)).Append("</code>")
                .AppendLine();
        }

        if (config.IncludeException && !string.IsNullOrWhiteSpace(logEntry.Exception))
        {
            sb.AppendLine()
                .Append("<b>Exception:</b> <code>").Append(Encode(logEntry.Exception)).Append("</code>")
                .AppendLine();
        }

        if (config.IncludeProperties && logEntry.Properties is { Count: > 0 })
        {
            sb.AppendLine().Append("<b>Properties:</b>").AppendLine();

            foreach (var property in logEntry.Properties)
            {
                sb.Append("<code>").Append(Encode(property.Key)).Append(": ").Append(Encode(property.Value))
                    .Append("</code>").AppendLine();
            }
        }

        return sb.ToString();
    }

    private static string Encode(string? value) => WebUtility.HtmlEncode(value) ?? string.Empty;
}
