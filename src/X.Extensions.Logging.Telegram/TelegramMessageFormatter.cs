using System;
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
            
            var level = _options.UseEmoji ? ToEmoji(logLevel) : ToString(logLevel);

            var sb = new StringBuilder();

            if (_options.UseEmoji)
            {
                sb.Append($"{level} *{DateTime.Now:hh:mm:ss}* {message}");
            }
            else
            {
                sb.Append($"*{DateTime.Now:hh:mm:ss}* {level}: {message}");    
            }
            
            sb.AppendLine();
            
            if (exception != null)
            {
                sb.AppendLine();
                sb.AppendLine($"`{exception}`");
                sb.AppendLine();
            }

            sb.Append($"_Initiator : {_name}_");
            
            if (!string.IsNullOrWhiteSpace(_options.Source))
            {
                sb.Append($"\t\t\t_Source: {_options.Source}_");
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