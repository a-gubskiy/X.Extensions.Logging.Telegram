using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram
{
    public class TelegramSink : TelegramSinkBase
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

    public class TelegramSinkBase : PeriodicBatchingSink
    {
        private readonly TelegramSinkMessageFormatter _messageFormatter;
        private readonly TelegramSinkConfiguration _config;
        private readonly ITelegramBotClient _botClient;

        public TelegramSinkBase(TelegramSinkConfiguration config)
            : base(config.BatchPostingLimit, config.BatchPeriod)
        {
            _config = config;
            _messageFormatter = new TelegramSinkMessageFormatter();
            _botClient = new TelegramBotClient(_config.Token);
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            await SendLog(events.Select(LogEntry.MapFrom));
        }

        protected async Task SendLog<T>(IEnumerable<T> objs) where T: LogEntry
        {
            await Task.Run(() =>
            {
                foreach (var obj in objs)
                {
                    var message = _messageFormatter.Format(
                        obj,
                        _config.FormatterConfiguration,
                        _messageFormatter.DefaultFormatter);
                    _botClient.SendTextMessageAsync(_config.ChatId, message, ParseMode.Markdown);
                }
            });
        }
    }
}