using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram
{
    public class TelegramLoggerProvider : ILoggerProvider
    {
        private static TelegramLoggerProcessor _telegramLoggerProcessor;
        
        private readonly TelegramLoggerOptions _options;
        
        private readonly ConcurrentDictionary<string, TelegramLogger> _loggers = new();

        public TelegramLoggerProvider(TelegramLoggerOptions options)
        {
            _options = options;

            if (_telegramLoggerProcessor == null)
            {
                _telegramLoggerProcessor = new TelegramLoggerProcessor(options);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new TelegramLogger(name, _options, _telegramLoggerProcessor));
        }

        public void Dispose() => _loggers.Clear();
    }
}