using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram;

internal class TelegramLoggerProvider : ILoggerProvider
{
    private readonly ITelegramLoggerProcessor _telegramLoggerProcessor;
        
    private readonly TelegramLoggerOptions _options;
        
    private readonly ConcurrentDictionary<string, TelegramLogger> _loggers = new();

    public TelegramLoggerProvider(TelegramLoggerOptions options, ITelegramLoggerProcessor telegramLoggerProcessor)
    {
        _options = options;
        _telegramLoggerProcessor = telegramLoggerProcessor;
    }

    public ILogger CreateLogger(string name)
    {
        return _loggers.GetOrAdd(name, CreateTelegramLogger);
    }

    private TelegramLogger CreateTelegramLogger(string name)
    {
        return new TelegramLogger(name, _options, _telegramLoggerProcessor, name);
    }

    public void Dispose() => _loggers.Clear();
}