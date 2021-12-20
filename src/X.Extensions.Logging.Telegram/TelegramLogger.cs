using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram;

public class TelegramLogger : ILogger
{
    private readonly ITelegramLoggerProcessor _queueProcessor;
    private readonly string _category;
    private readonly ITelegramMessageFormatter _formatter;

    internal TelegramLogger(
        string category,
        TelegramLoggerOptions options,
        ITelegramLoggerProcessor loggerProcessor)
        : this(category, options, loggerProcessor, new TelegramMessageFormatter(options, category))
    {
    }

    internal TelegramLogger(
        string category,
        TelegramLoggerOptions options,
        ITelegramLoggerProcessor loggerProcessor,
        ITelegramMessageFormatter formatter)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        
        _queueProcessor = loggerProcessor;
        _category = category;
        _formatter = formatter;
    }

    [PublicAPI]
    public TelegramLoggerOptions Options { get; private set; }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
            
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        var message = _formatter.Format(logLevel, eventId, state, exception, formatter);
            
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        _queueProcessor.EnqueueMessage(message);
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public IDisposable BeginScope<TState>(TState state) => default;
}