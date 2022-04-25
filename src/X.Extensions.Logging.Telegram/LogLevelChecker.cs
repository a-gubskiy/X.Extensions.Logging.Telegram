using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram;

public interface ILogLevelChecker
{
    bool IsEnabled(LogLevel logLevel);
}

public class DefaultLogLevelChecker : ILogLevelChecker
{
    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
}