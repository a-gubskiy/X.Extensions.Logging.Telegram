using System;
using System.Text;

using Serilog.Events;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram
{
    public class TelegramSinkMessageFormatter
    {
        public Func<LogEntry, TelegramMessageFormatterConfiguration, string> DefaultFormatter => DefaultMessageFormatter;

        public string Format(LogEntry entry, TelegramMessageFormatterConfiguration formatterConfig, Func<LogEntry, TelegramMessageFormatterConfiguration, string> formatter)
        {
            if (entry is null) throw new ArgumentNullException(nameof(entry));

            var message = formatter(entry, formatterConfig);
            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(nameof(message));
            }

            return message;
        }

        private string DefaultMessageFormatter(LogEntry entry, TelegramMessageFormatterConfiguration configuration)
        {
            if (entry is null) throw new ArgumentNullException(nameof(entry));

            var level = configuration.UseEmoji ? ToEmoji(entry.Level) : ToString(entry.Level);

            var sb = new StringBuilder();

            sb.Append("*[").AppendFormat("{0:G}", entry.UtcTimeStamp).Append(' ').Append(level).Append("]*").Append(' ').Append(configuration.ReadableApplicationName);

            sb.AppendLine();
            sb.AppendLine();
            sb.Append('*').Append("Message: ").Append('*').Append('`').Append(entry.RenderedMessage).Append('`').AppendLine();
            sb.Append('*').Append("Exception: ").Append('*').Append('`').Append(entry.Exception).Append('`').AppendLine();
            sb.Append('*').Append("Properties: ").Append('*').AppendLine()
                .Append('`').Append(entry.Properties).Append('`').AppendLine();

            return sb.ToString();
        }

        private string ToString(LogEventLevel logLevel)
        {
            return logLevel switch
            {
                LogEventLevel.Verbose => "VRB",
                LogEventLevel.Debug => "DBG",
                LogEventLevel.Information => "INF",
                LogEventLevel.Warning => "WARN",
                LogEventLevel.Error => "ERR",
                LogEventLevel.Fatal => "FTL",
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
            };
        }

        private string ToEmoji(LogEventLevel logLevel)
        {
            return logLevel switch
            {
                LogEventLevel.Verbose => "📝",
                LogEventLevel.Debug => "📓",
                LogEventLevel.Information => "ℹ️",
                LogEventLevel.Warning => "⚠️",
                LogEventLevel.Error => "❗",
                LogEventLevel.Fatal => "‼️",
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
            };
        }
    }
}