using JetBrains.Annotations;
using X.Extensions.Logging.Telegram.Base;

namespace X.Extensions.Serilog.Sinks.Telegram.Extensions;

/// <summary>
/// Helper methods for <see cref="LogEventLevel"/>.
/// </summary>
[PublicAPI]
public static class LogEventLevelExtensions
{
    /// <summary>
    /// Converts <see cref="LogEventLevel"/> to <see cref="TelegramLogLevel"/>.
    /// </summary>
    /// <param name="level">
    /// LogEventLevel value.
    /// </param>
    /// <returns>
    /// LogLevel value.
    /// </returns>
    public static TelegramLogLevel ToTelegramLogLevel(this LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => TelegramLogLevel.Trace,
            LogEventLevel.Debug => TelegramLogLevel.Debug,
            LogEventLevel.Information => TelegramLogLevel.Information,
            LogEventLevel.Warning => TelegramLogLevel.Warning,
            LogEventLevel.Error => TelegramLogLevel.Error,
            LogEventLevel.Fatal => TelegramLogLevel.Critical,
            _ => TelegramLogLevel.Information
        };
    }
}