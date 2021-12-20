using System.Collections.Generic;

namespace X.Extensions.Logging.Telegram.Tests;

public class FakeTelegramLoggerProcessor : ITelegramLoggerProcessor
{
    public List<string> Messages { get; private set; } = new List<string>();

    public void EnqueueMessage(string message) => Messages.Add(message);
    
    public void Dispose()
    {
    }
}