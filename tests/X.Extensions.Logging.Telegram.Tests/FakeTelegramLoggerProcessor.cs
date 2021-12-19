using System.Collections.Concurrent;
using System.Collections.Generic;

namespace X.Extensions.Logging.Telegram.Tests;

public class FakeTelegramLoggerProcessor : TelegramLoggerProcessor
{
    private readonly ConcurrentBag<string> _messages;
    
    public IReadOnlyCollection<string> Messages => _messages;

    public FakeTelegramLoggerProcessor(TelegramLoggerOptions options)
        : base(options.AccessToken, options.ChatId) => _messages = new ConcurrentBag<string>();

    public override void EnqueueMessage(string message) => _messages.Add(message);
}