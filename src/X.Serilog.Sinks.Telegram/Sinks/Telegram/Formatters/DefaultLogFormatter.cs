using System;
using System.Text;

using X.Serilog.Sinks.Telegram.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram.Formatters
{
    internal class DefaultLogFormatter : MessageFormatterBase
    {
        /// <inheritdoc cref="MessageFormatterBase"/>
        /// <exception cref="ArgumentNullException">Throws when the log entry is null.</exception>
        /// <exception cref="ArgumentException">Throws when, after using the formatter, the message is null, empty, or whitespace.</exception>
        public override string Format(
            LogEntry logEntry,
            FormatterConfiguration config,
            Func<LogEntry, FormatterConfiguration, string> formatter = null)
        {
            if (logEntry is null) throw new ArgumentNullException(nameof(logEntry));

            formatter ??= DefaultFormatter;

            var message = formatter(logEntry, config);
            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(nameof(message));
            }

            return message;
        }

        private string DefaultFormatter(LogEntry logEntry, FormatterConfiguration config)
        {
            if (logEntry is null) throw new ArgumentNullException(nameof(logEntry));

            var level = config.UseEmoji ? ToEmoji(logEntry.Level) : ToString(logEntry.Level);

            var sb = new StringBuilder();

            sb.Append("*[").AppendFormat("{0:G}", logEntry.UtcTimeStamp).Append(' ').Append(level).Append("]*").Append(' ').Append(config.ReadableApplicationName);

            sb.AppendLine();
            sb.AppendLine();

            if (NotEmpty(logEntry.RenderedMessage))
            {
                sb.Append('*').Append("Message: ").Append('*').Append('`').Append(logEntry.RenderedMessage).Append('`').AppendLine();
            }

            if (NotEmpty(logEntry.Exception))
            {
                sb.Append('*').Append("Exception: ").Append('*').Append('`').Append(logEntry.Exception).Append('`').AppendLine();
            }

            if (NotEmpty(logEntry.Properties))
            {
                sb.Append('*').Append("Properties: ").Append('*').AppendLine()
                    .Append('`').Append(logEntry.Properties).Append('`').AppendLine();
            }

            return sb.ToString();
        }
    }
}