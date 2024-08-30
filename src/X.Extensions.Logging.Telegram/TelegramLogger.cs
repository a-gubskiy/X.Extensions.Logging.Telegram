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
    private readonly ILogQueueProcessor _queueProcessor;
    private readonly IMessageFormatter _formatter;

    internal TelegramLogger(
        TelegramLoggerOptions options,
        ILogQueueProcessor loggerProcessor,
        IMessageFormatter formatter)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));

        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        
        _queueProcessor = loggerProcessor;
        
    }

    [PublicAPI]
    public TelegramLoggerOptions Options { get; private set; }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
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

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;
}