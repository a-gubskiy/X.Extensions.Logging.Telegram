using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram;

public class TelegramLogger : ILogger
{
    private readonly ILogLevelChecker _logLevelChecker;
    private readonly ILogQueueProcessor _queueProcessor;
    private readonly ITelegramMessageFormatter _formatter;

    internal TelegramLogger(
        TelegramLoggerOptions options,
        ILogLevelChecker  logLevelChecker,
        ILogQueueProcessor loggerProcessor,
        ITelegramMessageFormatter formatter)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        
        _logLevelChecker = logLevelChecker;
        _queueProcessor = loggerProcessor;
        _formatter = formatter;
    }

    [PublicAPI]
    public TelegramLoggerOptions Options { get; private set; }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
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

    public bool IsEnabled(LogLevel logLevel) => _logLevelChecker.IsEnabled(logLevel);

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;
}