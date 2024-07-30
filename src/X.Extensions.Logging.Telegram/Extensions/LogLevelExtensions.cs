using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Base;

namespace X.Extensions.Logging.Telegram.Extensions;

/// <summary>
/// Helper methods for <see cref="LogLevel"/>.
/// </summary>
[PublicAPI]
public static class LogLevelExtensions
{
    /// <summary>
    /// Converts <see cref="LogLevel"/> to <see cref="LogLevel"/>.
    /// </summary>
    /// <param name="level">
    /// LogEventLevel value.
    /// </param>
    /// <returns>
    /// LogLevel value.
    /// </returns>
    public static TelegramLogLevel ToTelegramLogLevel(this LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => TelegramLogLevel.Trace,
            LogLevel.Debug => TelegramLogLevel.Debug,
            LogLevel.Information => TelegramLogLevel.Information,
            LogLevel.Warning => TelegramLogLevel.Warning,
            LogLevel.Error => TelegramLogLevel.Error,
            LogLevel.Critical => TelegramLogLevel.Critical,
            _ => TelegramLogLevel.Information
        };
    }
}