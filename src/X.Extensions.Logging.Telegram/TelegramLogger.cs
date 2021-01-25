using System;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram
{
    public class TelegramLogger : ILogger
    {
        private readonly string _name;
        private readonly TelegramLoggerProcessor _queueProcessor;

        internal TelegramLogger(string name, TelegramLoggerOptions options, TelegramLoggerProcessor loggerProcessor)
        {
            _name = name;
            _queueProcessor = loggerProcessor;
            
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

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
            
            var message = Format(logLevel, eventId, state, exception, formatter);
            
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            _queueProcessor.EnqueueMessage(message);
        }

        private string Format<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);

            return $"[{ToString(logLevel)}] {message}";
        }

        private static string ToString(LogLevel level) =>
            level switch
            {
                LogLevel.Trace => "TRACE",
                LogLevel.Debug => "DEBUG",
                LogLevel.Information => "INFO",
                LogLevel.Warning => "WARN",
                LogLevel.Error => "ERROR",
                LogLevel.Critical => "CRITICAL",
                LogLevel.None => "NONE",
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
            };

        public bool IsEnabled(LogLevel logLevel) => logLevel >= Options.MinimumLogLevel;

        public IDisposable BeginScope<TState>(TState state) => default;
    }
}
