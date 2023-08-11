namespace X.Serilog.Sinks.Telegram.Configuration;

/// <summary>
/// Represents the configuration for formatting output messages.
/// </summary>
public class FormatterConfiguration
{
    /// <summary>
    /// Determines whether to replace the log level text definition with emojis.
    /// </summary>
    ///
    /// <value>
    /// A boolean representing the configuration. If true, emojis are used in place of log levels.
    /// </value>
    public bool UseEmoji { get; init; }

    /// <summary>
    /// Provides a readable name for the application. 
    /// </summary>
    ///
    /// <value>
    /// The application's name. Useful when different applications are sending logs to the same channel. If null, this property will be ignored.
    /// </value>
    public string? ReadableApplicationName { get; init; }

    /// <summary>
    /// Determines the handling of exceptions in log messages.
    /// </summary>
    ///
    /// <value>
    /// A boolean representing the configuration. If true, and an exception is not null, the exception is included in the log message as a serialized JSON. This setting is only applicable when TelegramSinkConfiguration.Mode is LoggingMode.Logs.
    /// </value>
    public bool IncludeException { get; init; }

    /// <summary>
    /// Specifies whether to include the log's parameters dictionary in log messages.
    /// </summary>
    ///
    /// <value>
    /// A boolean representing the configuration. If true, the log's parameters dictionary is included in the log message as a JSON. This setting is only applicable when TelegramSinkConfiguration.Mode is LoggingMode.Logs.
    /// </value>
    public bool IncludeProperties { get; init; }

    /// <summary>
    /// Sets or gets the time zone used by this sink for log timestamps. 
    /// </summary>
    ///
    /// <value>
    /// The time zone information. If null, server time will be used.
    /// </value>
    /// 
    /// <remarks>
    /// This property is specifically for this logger sink. It does not affect other sinks utilized by Serilog.
    /// Please note that each sink might handle log timestamps based on its individual configuration. To ensure the desired time zone is applied to your logs, you need to set this property for each necessary sink.
    /// </remarks>
    public TimeZoneInfo? TimeZone { get; init; }
}