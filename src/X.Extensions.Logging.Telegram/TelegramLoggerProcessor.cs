using System;
using System.Collections.Concurrent;
using System.Threading;

namespace X.Extensions.Logging.Telegram
{
    internal class TelegramLoggerProcessor : IDisposable
    {
        private const int MaxQueuedMessages = 1024;
        private const int Timeout = 1500;

        private readonly BlockingCollection<string> _queue = new(MaxQueuedMessages);
        private readonly Thread _outputThread;
        private readonly TelegramWriter _writer;

        public TelegramLoggerProcessor(TelegramLoggerOptions options)
        {
            _writer = new TelegramWriter(options.AccessToken, options.ChatId);
            
            // Start Telegram message queue processor
            _outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "Telegram logger queue processing thread"
            };
            
            _outputThread.Start();
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

        private void ProcessLogQueue()
        {
            try
            {
                foreach (var message in _queue.GetConsumingEnumerable())
                {
                    _writer.Write(message);
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
                _outputThread.Join(Timeout); // with timeout in-case Telegram is not respond
            }
            catch
            {
            }
        }
    }
    
}