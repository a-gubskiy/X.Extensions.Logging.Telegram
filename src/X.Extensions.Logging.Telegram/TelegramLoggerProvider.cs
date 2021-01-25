using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram
{
    internal class TelegramLoggerProvider : ILoggerProvider
    {
        private readonly TelegramLoggerProcessor _telegramLoggerProcessor;
        
        private readonly TelegramLoggerOptions _options;
        
        private readonly ConcurrentDictionary<string, TelegramLogger> _loggers = new();

        public TelegramLoggerProvider(TelegramLoggerOptions options, TelegramLoggerProcessor telegramLoggerProcessor)
        {
            _options = options;
            _telegramLoggerProcessor = telegramLoggerProcessor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new TelegramLogger(name, _options, _telegramLoggerProcessor));
        }

        public void Dispose() => _loggers.Clear();
    }
}