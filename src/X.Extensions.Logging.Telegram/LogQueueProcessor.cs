using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using X.Extensions.Logging.Telegram.Base;

namespace X.Extensions.Logging.Telegram;

[PublicAPI]
public interface ILogQueueProcessor : IDisposable
{
    void EnqueueMessage(string message);
}

internal class LogQueueProcessor : ILogQueueProcessor
{
    private const int MaxQueuedMessages = 1024;
    private const int Timeout = 1500;

    private readonly BlockingCollection<string> _queue = new(MaxQueuedMessages);
    private readonly Thread _thread;
    private readonly ILogWriter _writer;

    public LogQueueProcessor(ILogWriter logWriter)
    {
        _writer = logWriter;
            
        // Start message queue thread
        _thread = new Thread(async () => { await ProcessLogQueue(); })
        {
            IsBackground = true,
            Name = $"{nameof(LogQueueProcessor)} thread",
        };
            
        _thread.Start();
    }

    public void EnqueueMessage(string message)
    {
        if (!_queue.IsAddingCompleted)
        {
            try
            {
                _queue.Add(message);
                return;
            }
            catch
            {
                // ignored
            }
        }

        // Adding is completed so just log the message
        try
        {
            _writer.Write(message);
        }
        catch
        {
            // ignored
        }
    }

    private async Task ProcessLogQueue()
    {
        try
        {
            foreach (var message in _queue.GetConsumingEnumerable())
            {
                await _writer.Write(message);
            }
        }
        catch
        {
            try
            {
                _queue.CompleteAdding();
            }
            catch
            {
                // ignored
            }
        }
    }

    public void Dispose()
    {
        _queue.CompleteAdding();

        try
        {
            _thread.Join(Timeout); // with timeout in-case Telegram is not respond
        }
        catch
        {
            // ignored
        }
    }
}