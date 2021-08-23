using System;
using System.Collections;
using System.Linq;
using System.Text;

using Serilog.Events;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram
{
    internal class DefaultMessageFormatter : IMessageFormatter
    {
        /// <inheritdoc />>
        /// <exception cref="ArgumentNullException">Throws when the log entry is null.</exception>
        /// <exception cref="ArgumentException">Throws when, after using the formatter, the message is null, empty, or whitespace.</exception>
        public string Format(
            LogEntry entry,
            FormatterConfiguration formatterConfig,
            Func<LogEntry, FormatterConfiguration, string> formatter = null)
        {
            if (entry is null) throw new ArgumentNullException(nameof(entry));

            formatter ??= DefaultFormatter;

            var message = formatter(entry, formatterConfig);
            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(nameof(message));
            }

            return message;
        }

        private string DefaultFormatter(LogEntry entry, FormatterConfiguration configuration)
        {
            if (entry is null) throw new ArgumentNullException(nameof(entry));

            var level = configuration.UseEmoji ? ToEmoji(entry.Level) : ToString(entry.Level);

            var sb = new StringBuilder();

            sb.Append("*[").AppendFormat("{0:G}", entry.UtcTimeStamp).Append(' ').Append(level).Append("]*").Append(' ').Append(configuration.ReadableApplicationName);

            sb.AppendLine();
            sb.AppendLine();

            if (NotEmpty(entry.RenderedMessage))
            {
                sb.Append('*').Append("Message: ").Append('*').Append('`').Append(entry.RenderedMessage).Append('`').AppendLine();
            }

            if (NotEmpty(entry.Exception))
            {
                sb.Append('*').Append("Exception: ").Append('*').Append('`').Append(entry.Exception).Append('`').AppendLine();
            }

            if (NotEmpty(entry.Properties))
            {
                sb.Append('*').Append("Properties: ").Append('*').AppendLine()
                    .Append('`').Append(entry.Properties).Append('`').AppendLine();
            }

            return sb.ToString();
        }

#pragma warning disable CA1822
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private bool NotEmpty<T>(T value)
        {
            switch (value) {
                case null:
                case string s when string.IsNullOrWhiteSpace(s):
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                case IEnumerable e when !e.Cast<object>().Any():
                    return false;
            }

            return true;
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
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

        // ReSharper disable once MemberCanBeMadeStatic.Local
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
#pragma warning restore CA1822
    }
}