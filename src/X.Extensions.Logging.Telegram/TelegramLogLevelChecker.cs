using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram;

public interface ITelegramLogLevelChecker
{
    bool IsEnabled(LogLevel logLevel);
}

public class TelegramLogLevelChecker : ITelegramLogLevelChecker
{
    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
}