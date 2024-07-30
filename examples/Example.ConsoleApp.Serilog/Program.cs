using Example.Core;
using Serilog;
using Serilog.Events;
using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Extensions;
using X.Extensions.Serilog.Sinks.Telegram.Filters.Fluent;

ConfigAsMinimal(ExampleAppSettings.Token, ExampleAppSettings.ChatId);

var logsCounter = 0;
const int logsThreshold = 100;

while (logsCounter <= logsThreshold)
{
    var level = Random.Shared.NextInt64(0, 6);
    Log.Logger.Write((LogEventLevel)level, "Message {counter}", logsCounter);
    await Task.Delay(500);

    logsCounter++;
}

return;

void ConfigAsMinimal(string token, string tgChatId)
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.TelegramCore(
            token: token,
            chatId: tgChatId,
            logLevel: LogEventLevel.Verbose)
        .CreateLogger();
}

void ConfigAsExtended(string token, string tgChatId)
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Telegram(config =>
        {
            config.Token = token;
            config.ChatId = tgChatId;

            config.Mode = LoggingMode.Logs;

            config.BatchPostingLimit = TelegramSinkDefaults.BatchPostingLimit;
            config.BatchEmittingRulesConfiguration = new BatchEmittingRulesConfiguration();
            config.FormatterConfiguration = new FormatterConfiguration
            {
                UseEmoji = true,
                ReadableApplicationName = "MyTestApp",
                IncludeException = true,
                IncludeProperties = true,
                TimeZone = TimeZoneInfo.Utc
            };
            config.LogFiltersConfiguration = new LogsFiltersConfiguration
            {
                ApplyLogFilters = true,
                QueryBuilder = LogQueryBuilder.Create()
                    .Exception.NotNull()
                    .And().Level.Equals(LogEventLevel.Fatal)
                    .And().Message.Contains("Payment API failed")
            };
        }, null!, LogEventLevel.Debug)
        .WriteTo.Console()
        .CreateLogger();
}