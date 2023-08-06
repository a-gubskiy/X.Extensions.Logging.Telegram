using Serilog.Sinks.PeriodicBatching;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Formatters;

namespace X.Serilog.Sinks.Telegram;

public class TelegramSink : TelegramSinkBase
{
    public TelegramSink(IMessageFormatter messageFormatter, TelegramSinkConfiguration config)
        : base(messageFormatter, config)
    {
    }

    protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
    {
        await SendLog(events.Select(LogEntry.MapFrom));
    }
}

public class TelegramSinkBase : PeriodicBatchingSink
{
    private readonly ITelegramBotClient _botClient;
    private readonly TelegramSinkConfiguration _config;
    private readonly IMessageFormatter _messageFormatter;

    protected TelegramSinkBase(IMessageFormatter messageFormatter, TelegramSinkConfiguration config)
        : base(config.BatchPostingLimit, config.BatchPeriod)
    {
        _config = config;
        _messageFormatter = messageFormatter ?? TelegramSinkDefaults.GetDefaultMessageFormatter(_config.Mode);
        _botClient = new TelegramBotClient(_config.Token);
    }

    protected async Task SendLog<T>(IEnumerable<T> logEntries) where T : LogEntry
    {
        var messages = _messageFormatter.Format(
            logEntries.ToList() as ICollection<LogEntry>,
            _config.FormatterConfiguration);

        foreach (var message in messages)
        {
            await _botClient.SendTextMessageAsync(_config.ChatId, message, parseMode: ParseMode.Html);
        }
    }
}