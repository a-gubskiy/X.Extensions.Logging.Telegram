using X.Serilog.Sinks.Telegram.Formatters;

namespace X.Serilog.Sinks.Telegram.Configuration;

/// <summary>
/// Contains the default configuration values for the Telegram Sink.
/// </summary>
public static class TelegramSinkDefaults
{
    /// <summary>
    /// The limit on the number of log events to be included in a single batch.
    /// </summary>
    public const int BatchPostingLimit = 20;

    /// <summary>
    /// The period between consecutive checks of rules.
    /// </summary>
    public static readonly TimeSpan RulesCheckPeriod = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Retrieves the default message formatter according to the specified logging mode.
    /// </summary>
    /// <param name="loggingMode">The logging mode.</param>
    /// <returns>A message formatter.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid logging mode is supplied.</exception>
    internal static IMessageFormatter GetDefaultMessageFormatter(LoggingMode loggingMode)
    {
        return loggingMode switch
        {
            LoggingMode.Logs => new DefaultLogFormatter(),
            LoggingMode.AggregatedNotifications => new DefaultAggregatedNotificationsFormatter(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}