using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Logging.Telegram.Base.Formatters;

namespace X.Extensions.Serilog.Sinks.Telegram.Configuration;

/// <summary>
/// Contains the default configuration values for the Telegram Sink.
/// </summary>
public static class TelegramSinkDefaults
{
    /// <summary>
    /// Gets the logging mode for the application.
    /// </summary>
    /// <value>
    /// The logging mode determines how log messages are processed and formatted before being sent. 
    /// In this case, it is set to return <see cref="LoggingMode.Logs"/>, which indicates that log messages 
    /// will be published individually to the specified Telegram channel.
    /// </value>
    /// <remarks>
    /// This property is read-only and returns the default logging mode of the system. 
    /// It is crucial for configuring the overall logging strategy of the application. 
    /// For example, when set to <see cref="LoggingMode.Logs"/>, each log message is sent as it occurs. 
    /// Other modes, like <see cref="LoggingMode.AggregatedNotifications"/>, could aggregate messages over a period 
    /// or until a certain condition is met before sending.
    /// </remarks>
    public static LoggingMode DefaultFormatterMode => LoggingMode.AggregatedNotifications;

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
    public static IMessageFormatter GetDefaultLogFormatter(LoggingMode loggingMode)
    {
        return loggingMode switch
        {
            LoggingMode.Logs => new DefaultLogFormatter(),
            LoggingMode.AggregatedNotifications => new DefaultAggregatedNotificationsFormatter(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}