using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Base.Formatters;

namespace X.Extensions.Logging.Telegram;

internal class TelegramLoggerProvider : ILoggerProvider
{
    private readonly ILogQueueProcessor _logQueueProcessor;
    private readonly ILogLevelChecker _logLevelChecker;
    private readonly TelegramLoggerOptions _options;
    private readonly ConcurrentDictionary<string, TelegramLogger> _loggers = new();
    private readonly Func<string, IMessageFormatter> _createFormatter;
    
    public TelegramLoggerProvider(
        TelegramLoggerOptions options,
        ILogQueueProcessor logQueueProcessor,
        ILogLevelChecker logLevelChecker, 
        Func<string, IMessageFormatter> createFormatter)
    {
        _options = options;
        _logQueueProcessor = logQueueProcessor;
        _logLevelChecker = logLevelChecker;
        _createFormatter = createFormatter;
    }
    
    public ILogger CreateLogger(string name) => _loggers.GetOrAdd(name, CreateTelegramLogger);

    private TelegramLogger CreateTelegramLogger(string name) =>
        new TelegramLogger(_options, _logLevelChecker, _logQueueProcessor, _createFormatter(name));

    public void Dispose() => _loggers.Clear();
}