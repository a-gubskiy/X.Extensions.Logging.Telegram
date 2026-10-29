using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using X.Extensions.Logging.Telegram.Base;

namespace X.Extensions.Logging.Telegram;

[PublicAPI]
public interface ILogQueueProcessor : IDisposable
{
    void EnqueueMessages(ICollection<string> messages);
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

        _thread = new Thread(ProcessLogQueue)
        {
            IsBackground = true,
            Name = $"{nameof(LogQueueProcessor)} thread",
        };

        _thread.Start();
    }

    public void EnqueueMessages(ICollection<string> messages)
    {
        foreach (var message in messages)
        {
            try
            {
                _queue.Add(message);
            }
            catch (InvalidOperationException)
            {
                // Adding is completed (processor is disposed), so the message is dropped
            }
        }
    }

    private void ProcessLogQueue()
    {
        foreach (var message in _queue.GetConsumingEnumerable())
        {
            try
            {
                _writer.Write(message).GetAwaiter().GetResult();
            }
            catch
            {
                // ignored: logging must never crash the application
            }
        }
    }

    public void Dispose()
    {
        _queue.CompleteAdding();

        // Give the queue some time to flush in case Telegram does not respond
        _thread.Join(Timeout);
    }
}
