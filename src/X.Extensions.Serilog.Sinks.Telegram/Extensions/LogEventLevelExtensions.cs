using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Serilog.Sinks.Telegram.Extensions;

/// <summary>
/// Helper methods for <see cref="LogEventLevel"/>.
/// </summary>
[PublicAPI]
public static class LogEventLevelExtensions
{
    /// <summary>
    /// Converts <see cref="LogEventLevel"/> to <see cref="LogLevel"/>.
    /// </summary>
    /// <param name="level">
    /// LogEventLevel value.
    /// </param>
    /// <returns>
    /// LogLevel value.
    /// </returns>
    public static LogLevel ToLogLevel(this LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => LogLevel.Information
        };
    }
}