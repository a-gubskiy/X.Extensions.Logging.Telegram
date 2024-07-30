using System;
using System.Collections.Generic;
using X.Extensions.Logging.Telegram.Base;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;

namespace X.Extensions.Serilog.Sinks.Telegram.Formatters;

public interface IMessageFormatter
{
    /// <summary>
    ///     Creates a human-readable message from a <see cref="IEnumerable{LogEntry}"/>.
    /// </summary>
    /// <param name="logEntries">Collection of <see cref="LogEntry"/> which provides an info for the message.</param>
    /// <param name="config">Formatter configuration.</param>
    /// <param name="formatter">The function would be used to create a message.</param>
    /// <returns>Human-readable message.</returns>
    List<string> Format(ICollection<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<ICollection<LogEntry>, FormatterConfiguration, List<string>>? formatter = null);
}