namespace X.Extensions.Logging.Telegram.Tests;

public class FakeLogQueueProcessor : ILogQueueProcessor
{
    public List<string> Messages { get; private set; } = new List<string>();

    public void EnqueueMessage(string message) => Messages.Add(message);
    
    public void Dispose()
    {
    }
}