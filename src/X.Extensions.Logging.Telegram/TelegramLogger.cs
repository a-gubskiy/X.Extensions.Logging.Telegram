using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Base;
using X.Extensions.Logging.Telegram.Base.Formatters;
using X.Extensions.Logging.Telegram.Extensions;

namespace X.Extensions.Logging.Telegram;

public class TelegramLogger : ILogger
{
    private readonly ILogLevelChecker _logLevelChecker;
    private readonly ILogQueueProcessor _queueProcessor;
    private readonly ILogFormatter _formatter;

    internal TelegramLogger(
        TelegramLoggerOptions options,
        ILogLevelChecker  logLevelChecker,
        ILogQueueProcessor loggerProcessor,
        ILogFormatter formatter)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        
        _logLevelChecker = logLevelChecker;
        _queueProcessor = loggerProcessor;
        _formatter = formatter;
    }

    [PublicAPI]
    public TelegramLoggerOptions Options { get; private set; }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
            
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        ICollection<LogEntry> logEntries = new List<LogEntry>
        {
            CreateLogEntry(logLevel, state, exception, formatter)
        };
        
        var messages = _formatter.Format(logEntries, Options.FormatterConfiguration);
            
        if (!messages.Any())
        {
            return;
        }

        _queueProcessor.EnqueueMessages(messages);
    }

    private static LogEntry CreateLogEntry<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        return new LogEntry
        {
            Exception = exception?.ToString(),
            Level = logLevel.ToTelegramLogLevel(),
            Message = formatter.Invoke(state, exception),
            Properties = new Dictionary<string, string>(),
            UtcTimeStamp = DateTime.UtcNow
        };
    }

    public bool IsEnabled(LogLevel logLevel) => _logLevelChecker.IsEnabled(logLevel);

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;
}