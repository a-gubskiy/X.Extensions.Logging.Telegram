using System.Collections;
using X.Extensions.Logging.Telegram;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;

namespace X.Extensions.Serilog.Sinks.Telegram.Formatters;

public abstract class MessageFormatterBase : IMessageFormatter
{
    protected static readonly List<string> Empty = Enumerable.Empty<string>().ToList();
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

        var messages = formatter(logEntries, config);
        messages = messages.Where(msg => !string.IsNullOrEmpty(msg)).ToList();
        return messages;
    }

    protected virtual bool NotEmpty<T>(T value)
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
}