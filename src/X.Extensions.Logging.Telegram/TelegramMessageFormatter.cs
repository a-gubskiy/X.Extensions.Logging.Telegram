using System;
using System.Net;
using System.Text;

using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram
{
    public class TelegramMessageFormatter
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
            Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

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
            sb.AppendLine($"Message: {message}");
            sb.AppendLine();

            if (exception != null)
            {
                sb.AppendLine();
                sb.AppendLine($"<pre>{WebUtility.HtmlEncode(exception.ToString())}</pre>");
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
}