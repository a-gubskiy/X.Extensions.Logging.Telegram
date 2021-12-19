using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace X.Extensions.Logging.Telegram;

public class TelegramLoggerProcessor : IDisposable
{
    private const int MaxQueuedMessages = 1024;
    private const int Timeout = 1500;

    private readonly BlockingCollection<string> _queue = new(MaxQueuedMessages);
    private readonly Thread _thread;
    private readonly ITelegramWriter _writer;

    public TelegramLoggerProcessor(string accessToken, string chatId)
    {
        _writer = new TelegramWriter(accessToken, chatId);
            
        // Start Telegram message queue process
        _thread = new Thread(async () => { await ProcessLogQueue(); })
        {
            IsBackground = true,
            Name = "Telegram logger queue process thread",
        };
            
        _thread.Start();
    }

    public virtual void EnqueueMessage(string message)
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
            }
        }

        // Adding is completed so just log the message
        try
        {
            _writer.Write(message);
        }
        catch
        {
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
        }
    }
}