using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram
{
    public class TelegramLogger : ILogger
    {
        private readonly TelegramLoggerProcessor _queueProcessor;
        private readonly string _category;
        private readonly TelegramMessageFormatter _formatter;

        internal TelegramLogger(string name, TelegramLoggerOptions options, TelegramLoggerProcessor loggerProcessor, string category)
        {
            _queueProcessor = loggerProcessor;
            _category = category;
            _formatter = new TelegramMessageFormatter(options, name);

            Options = options ?? throw new ArgumentNullException(nameof(options));
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

        public bool IsEnabled(LogLevel logLevel) => 
            logLevel > Options.LogLevel || 
            logLevel == Options.LogLevel && (Options.Categories == null || Options.Categories.Contains(_category));

        public IDisposable BeginScope<TState>(TState state) => default;
    }
}
