using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram
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
            _messageFormatter = _config.FormatterConfiguration.Formatter ?? new DefaultMessageFormatter();
            _botClient = new TelegramBotClient(_config.Token);
        }

        protected async Task SendLog<T>(IEnumerable<T> logEntries) where T: LogEntry
        {
            await Task.Run(() =>
            {
                foreach (var entry in logEntries)
                {
                    var message = _messageFormatter.Format(entry, _config.FormatterConfiguration);
                    _botClient.SendTextMessageAsync(_config.ChatId, message, ParseMode.Markdown);
                }
            });
        }
    }
}