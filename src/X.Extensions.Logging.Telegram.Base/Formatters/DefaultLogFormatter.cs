using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Formatters;

namespace X.Extensions.Logging.Telegram.Base.Formatters;

public class DefaultLogFormatter : TelegramMessageFormatterBase
{
    /// <inheritdoc cref="TelegramMessageFormatterBase"/>
    /// <exception cref="ArgumentNullException">Throws when the log entry is null.</exception>
    /// <exception cref="ArgumentException">Throws when, after using the formatter, the message is null, empty, or whitespace.</exception>
    public override List<string> Format(ICollection<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<ICollection<LogEntry>, FormatterConfiguration, List<string>>? formatter = null)
    {
        if (!NotEmpty(logEntries))
        {
            return Empty;
        }

        formatter ??= DefaultFormatter;

        var logEntryWrapper = new LogEntry[1];
        
        if (logEntries.Count is 1)
        {
            return base.Format(logEntries, config, formatter);
        }

        var formattedMessages = new List<string>();
        
        foreach (var logEntry in logEntries)
        {
            logEntryWrapper[0] = logEntry;
            var messages = base.Format(logEntryWrapper, config, formatter);
            
            formattedMessages.AddRange(messages);
        }

        return formattedMessages;
    }

    private List<string> DefaultFormatter(ICollection<LogEntry> logEntries, FormatterConfiguration config)
    {
        if (logEntries.Count > 1)
        {
            throw new ArgumentException("Formatter supports only single element collections", nameof(logEntries));
        }

        var formattedMessage = FormatMessageInternal(logEntries.First(), config);
        
        return new List<string> { formattedMessage };
    }

    private string FormatMessageInternal(LogEntry logEntry, FormatterConfiguration config)
    {
        if (logEntry is null) 
        {
            throw new ArgumentNullException(nameof(logEntry));
        }

        var sb = new StringBuilder();
        var timestamp = config.TimeZone != null
            ? TimeZoneInfo.ConvertTime(logEntry.UtcTimeStamp, config.TimeZone)
            : logEntry.UtcTimeStamp;

        var logLevelMarker = config.UseEmoji
            ? LogLevelMarkerRenderer.RenderMarker(logEntry.Level) + logEntry.Level
            : logEntry.Level.ToString();

        sb.Append(logLevelMarker)
            .Append(' ').Append('[').Append(config.ReadableApplicationName ?? "YourApp").Append(']')
            .Append(' ').Append('[').Append($"{timestamp:yyyy-MM-dd HH:mm:ss UTC}").Append(']')
            .AppendLine();

        if (NotEmpty(logEntry.Message))
        {
            sb.AppendLine();
            sb.Append("<b>Message:</b> <code>").Append(WebUtility.HtmlEncode(logEntry.Message)).Append("</code>");
            sb.AppendLine();
        }

        if (config.IncludeException && NotEmpty(logEntry.Exception))
        {            
            sb.AppendLine();
            sb.Append("Exception: `").Append(WebUtility.HtmlEncode(logEntry.Exception)).Append("`");
            sb.AppendLine();
        }

        if (config.IncludeProperties && logEntry.Properties != null && logEntry.Properties.Count != 0)
        {
            sb.AppendLine().Append("<b>").Append("Properties: ").Append("</b>").AppendLine();
            
            foreach (var property in logEntry.Properties)
            {
                sb.Append("<code>").Append(property.Key).Append(": ").Append(property.Value).Append("</code>").AppendLine();
            }
        }

        return sb.ToString();
    }
}