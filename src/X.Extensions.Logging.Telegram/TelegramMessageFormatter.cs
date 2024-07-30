using System;
using System.Net;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Extensions;
using X.Extensions.Telegram;

namespace X.Extensions.Logging.Telegram;

[PublicAPI]
public interface ITelegramMessageFormatter
{
    string Format<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter);

    string Format(LogLevel logLevel, Exception? exception, string message);
        
    string EncodeHtml(string text);
}

[PublicAPI]
public class TelegramMessageFormatter : ITelegramMessageFormatter
{
    private readonly TelegramLoggerOptions _options;
    private readonly string _category;

    public TelegramMessageFormatter(TelegramLoggerOptions options, string category)
    {
        _options = options;
        _category = category;
    }

    public string Format<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter) =>
        Format(logLevel, exception, formatter(state, exception));

    public virtual string Format(LogLevel logLevel, Exception? exception, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return string.Empty;
        }

        ILogLevelMarkerRenderer logLevelMarkerRenderer =
            _options.UseEmoji ? new LogLevelEmojiMarkerRenderer() : new LogLevelTextMarkerRenderer(); 
            
        var sb = new StringBuilder();

        sb.Append($"<b>{logLevelMarkerRenderer.RenderMarker(logLevel.ToTelegramLogLevel())} {DateTime.Now:HH:mm:ss}</b>");

        sb.AppendLine();
        sb.Append($"<i>{_category}</i>");

        sb.AppendLine();
        sb.AppendLine($"Message: {EncodeHtml(message)}");
        sb.AppendLine();

        if (exception != null)
        {
            sb.AppendLine();
            sb.AppendLine($"<pre>{EncodeHtml(exception.ToString())}</pre>");
            sb.AppendLine();
        }

        if (!string.IsNullOrWhiteSpace(_options.Source))
        {
            sb.AppendLine();
            sb.Append($"<i>Source: {_options.Source}</i>");
        }
            
        sb.AppendLine();
            
        return sb.ToString();
    }

    public virtual string EncodeHtml(string text) => WebUtility.HtmlEncode(text);   
}