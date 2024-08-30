namespace X.Extensions.Logging.Telegram.Tests;

public class FakeLogQueueProcessor : ILogQueueProcessor
{
    public List<string> Messages { get; private set; } = new List<string>();

    public void EnqueueMessages(ICollection<string> messages) => Messages.AddRange(messages);
    
    public void Dispose()
    {
    }
}