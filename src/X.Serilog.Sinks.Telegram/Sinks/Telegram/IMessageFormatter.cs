using System;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram
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
    }
}