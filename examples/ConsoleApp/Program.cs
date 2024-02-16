using Serilog;
using Serilog.Events;
using X.Serilog.Sinks.Telegram.Extensions;

const string botToken = "TELEGRAM_BOT_TOKEN";
const string loggingChatId = "CHANNEL_OR_CHAT_ID";

Log.Logger = new LoggerConfiguration()
    .WriteTo.TelegramCore(
        token: botToken,
        chatId: loggingChatId,
        logLevel: LogEventLevel.Verbose)
    .WriteTo.Console()
    .CreateLogger();


while (true)
{
    var level = Random.Shared.NextInt64(0, 6);
    Log.Logger.Write((LogEventLevel)level, "Message");
    await Task.Delay(500);
}