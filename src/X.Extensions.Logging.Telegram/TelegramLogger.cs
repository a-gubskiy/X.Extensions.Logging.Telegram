using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Base;
using X.Extensions.Logging.Telegram.Base.Formatters;
using X.Extensions.Logging.Telegram.Extensions;

namespace X.Extensions.Logging.Telegram;

public class TelegramLogger : ILogger
{
    private readonly ILogQueueProcessor _queueProcessor;
    private readonly IMessageFormatter _formatter;

    internal TelegramLogger(
        TelegramLoggerOptions options,
        ILogQueueProcessor queueProcessor,
        IMessageFormatter formatter)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        _queueProcessor = queueProcessor ?? throw new ArgumentNullException(nameof(queueProcessor));
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
    }

    [PublicAPI]
    public TelegramLoggerOptions Options { get; }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var logEntry = new LogEntry
        {
            Exception = exception?.ToString(),
            Level = logLevel.ToTelegramLogLevel(),
            Message = formatter(state, exception),
            Properties = [],
            UtcTimeStamp = DateTime.UtcNow
        };

        var messages = _formatter.Format([logEntry], Options.FormatterConfiguration);

        if (messages.Count == 0)
        {
            return;
        }

        _queueProcessor.EnqueueMessages(messages);
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}
