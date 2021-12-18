using System;
using System.Net;
using System.Text;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram;

[PublicAPI]
public interface ITelegramMessageFormatter
{
    string Format<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter);

    string Format(LogLevel logLevel, Exception exception, string message);
        
    string EncodeHtml(string text);
}

[PublicAPI]
public class TelegramMessageFormatter : ITelegramMessageFormatter
{
    private readonly TelegramLoggerOptions _options;
    private readonly string _name;

    public TelegramMessageFormatter(TelegramLoggerOptions options, string name)
    {
        _options = options;
        _name = name;
    }

    public string Format<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter) =>
        Format(logLevel, exception, formatter(state, exception));

    public virtual string Format(LogLevel logLevel, Exception exception, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return string.Empty;
        }
            
        var sb = new StringBuilder();

        sb.Append(_options.UseEmoji
            ? $"{ToEmoji(logLevel)} <b>{DateTime.Now:HH:mm:ss}</b> {ToString(logLevel)}"
            : $"<b>{DateTime.Now:HH:mm:ss}</b> {ToString(logLevel)}");

        sb.AppendLine();
        sb.Append($"<pre>{_name}</pre>");

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

    private static string ToString(LogLevel level) =>
        level switch
        {
            LogLevel.Trace => "TRACE",
            LogLevel.Debug => "DEBUG",
            LogLevel.Information => "INFO",
            LogLevel.Warning => "️️WARN",
            LogLevel.Error => "ERROR",
            LogLevel.Critical => "CRITICAL",
            LogLevel.None => " ",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
   
    private static string ToEmoji(LogLevel level) =>
        level switch
        {
            LogLevel.Trace => "⬜️",
            LogLevel.Debug => "🟦",
            LogLevel.Information => "⬛️️️",
            LogLevel.Warning => "🟧",
            LogLevel.Error => "🟥",
            LogLevel.Critical => "❌",
            LogLevel.None => "🔳",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
}