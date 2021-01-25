using System;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram
{
    public class TelegramLogger : ILogger
    {
        private readonly TelegramLoggerProcessor _queueProcessor;
        private readonly TelegramMessageFormatter _formatter;

        internal TelegramLogger(string name, TelegramLoggerOptions options, TelegramLoggerProcessor loggerProcessor)
        {
            _queueProcessor = loggerProcessor;
            _formatter = new TelegramMessageFormatter(options, name);

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

            var message = _formatter.Format(logLevel, eventId, state, exception, formatter);
            
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            _queueProcessor.EnqueueMessage(message);
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= Options.MinimumLogLevel;

        public IDisposable BeginScope<TState>(TState state) => default;
    }
}
