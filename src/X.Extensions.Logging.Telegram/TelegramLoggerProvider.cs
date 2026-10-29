using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Base.Formatters;

namespace X.Extensions.Logging.Telegram;

internal class TelegramLoggerProvider(
    TelegramLoggerOptions options,
    ILogQueueProcessor logQueueProcessor,
    Func<string, IMessageFormatter> createFormatter) : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, TelegramLogger> _loggers = new();

    public ILogger CreateLogger(string name) =>
        _loggers.GetOrAdd(name, category => new TelegramLogger(options, logQueueProcessor, createFormatter(category)));

    public void Dispose()
    {
        _loggers.Clear();
        logQueueProcessor.Dispose();
    }
}
