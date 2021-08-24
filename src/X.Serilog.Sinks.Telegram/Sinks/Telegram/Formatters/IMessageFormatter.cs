using System;
using System.Collections.Generic;

using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Formatters
{
    public interface IMessageFormatter
    {
        /// <summary>
        ///     Creates a human-readable message from a <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="logEntry">Info that should be logged, represented with a model.</param>
        /// <param name="config">Formatter configuration.</param>
        /// <param name="formatter">The function would be used to create a message.</param>
        /// <returns>Human-readable message.</returns>
        string Format(
            LogEntry logEntry,
            FormatterConfiguration config,
            Func<LogEntry, FormatterConfiguration, string> formatter = null);

        /// <summary>
        ///     Creates a human-readable message from a <see cref="IEnumerable{LogEntry}"/>.
        /// </summary>
        /// <param name="logEntries">Collection of <see cref="LogEntry"/> which provides an info for the message.</param>
        /// <param name="config">Formatter configuration.</param>
        /// <param name="formatter">The function would be used to create a message.</param>
        /// <returns>Human-readable message.</returns>
        string Format(IEnumerable<LogEntry> logEntries,
            FormatterConfiguration config,
            Func<IEnumerable<LogEntry>, FormatterConfiguration, string> formatter = null);
    }
}