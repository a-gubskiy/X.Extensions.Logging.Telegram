using System.Collections.Concurrent;
using X.Extensions.Logging.Telegram.Base;

namespace X.Extensions.Logging.Telegram.Tests;

public class FakeLogWriter : ILogWriter
{
    public ConcurrentQueue<string> Messages { get; } = new();

    public Task Write(string message) => Write(message, CancellationToken.None);

    public async Task Write(string message, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        Messages.Enqueue(message);
    }
}
