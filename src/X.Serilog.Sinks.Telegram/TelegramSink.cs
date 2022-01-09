using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Formatters;

namespace X.Serilog.Sinks.Telegram
{
    internal class TelegramSink : TelegramSinkBase
    {
        public TelegramSink(TelegramSinkConfiguration config)
            : base(config)
        {
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            await SendLog(events.Select(LogEntry.MapFrom));
        }
    }

    internal class TelegramSinkBase : PeriodicBatchingSink
    {
        private readonly IMessageFormatter _messageFormatter;
        private readonly TelegramSinkConfiguration _config;
        private readonly ITelegramBotClient _botClient;

        protected TelegramSinkBase(TelegramSinkConfiguration config)
            : base(config.BatchPostingLimit, config.BatchPeriod)
        {
            _config = config;
            _messageFormatter = _config.FormatterConfiguration.Formatter ?? GetMessageFormatter();
            _botClient = new TelegramBotClient(_config.Token);
        }

        private IMessageFormatter GetMessageFormatter()
        {
            return _config.Mode switch
            {
                LoggingMode.Logs => new DefaultLogFormatter(),
                LoggingMode.Notifications => new DefaultNotificationFormatter(),
                LoggingMode.AggregatedNotifications => new DefaultAggregatedNotificationsFormatter(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected async Task SendLog<T>(IEnumerable<T> logEntries) where T: LogEntry
        {
            await Task.Run(() =>
            {
                var messages = GetMessages(logEntries.ToList());
                foreach (var message in messages)
                {
                    _botClient.SendTextMessageAsync(_config.ChatId, message, ParseMode.Markdown);
                }
            });
        }

        private List<string> GetMessages(IReadOnlyCollection<LogEntry> entries)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (_config.Mode)
            {
                case LoggingMode.Logs:
                case LoggingMode.Notifications:
                    var messages = new List<string>(entries.Count);
                    messages.AddRange(entries.Select(entry =>
                        _messageFormatter.Format(entry, _config.FormatterConfiguration)));

                    return messages;
                case LoggingMode.AggregatedNotifications:
                    return new List<string>(1)
                    {
                        _messageFormatter.Format(entries, _config.FormatterConfiguration),
                    };
            }

            return new List<string>();
        }
    }
}