using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Logging.Telegram.Base.Rendereres;

namespace X.Extensions.Logging.Telegram.Base.Formatters;

public abstract class MessageFormatterBase : IMessageFormatter
{
    protected static readonly List<string> Empty = [];
    protected static readonly ILogLevelMarkerRenderer LogLevelMarkerRenderer = new LogLevelEmojiMarkerRenderer();

    /// <inheritdoc />
    public virtual List<string> Format(ICollection<LogEntry> logEntries,
        FormatterConfiguration config,
        Func<ICollection<LogEntry>, FormatterConfiguration, List<string>>? formatter = null)
    {
        if (!NotEmpty(logEntries))
        {
            throw new ArgumentException(null, nameof(logEntries));
        }

        if (formatter is null)
        {
            return Empty;
        }

        return formatter(logEntries, config)
            .Where(message => !string.IsNullOrEmpty(message))
            .ToList();
    }

    protected virtual bool NotEmpty<T>(T value)
    {
        switch (value)
        {
            case null:
            case string s when string.IsNullOrWhiteSpace(s):
            case ICollection { Count: 0 }:
            case IEnumerable e when !e.Cast<object>().Any():
                return false;
        }

        return true;
    }
}
