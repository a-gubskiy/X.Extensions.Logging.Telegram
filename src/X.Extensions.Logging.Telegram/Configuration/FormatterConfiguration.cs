using System;

namespace X.Extensions.Logging.Telegram.Base.Configuration;

/// <summary>
/// Represents the configuration for formatting output messages.
/// </summary>
public record FormatterConfiguration
{
    /// <summary>
    /// Determines whether to prefix the log level with an emoji.
    /// </summary>
    public bool UseEmoji { get; set; }

    /// <summary>
    /// Provides a readable name for the application.
    /// Useful when different applications are sending logs to the same channel.
    /// </summary>
    public string? ReadableApplicationName { get; set; }

    /// <summary>
    /// Determines whether to include the exception in log messages.
    /// </summary>
    public bool IncludeException { get; set; }

    /// <summary>
    /// Determines whether to include the log entry properties in log messages.
    /// </summary>
    public bool IncludeProperties { get; set; }

    /// <summary>
    /// The time zone used for log timestamps. If null, UTC is used.
    /// </summary>
    public TimeZoneInfo? TimeZone { get; set; }

    public static FormatterConfiguration Default => new()
    {
        UseEmoji = true,
        ReadableApplicationName = "YourApp",
        IncludeException = false,
        IncludeProperties = false,
        TimeZone = TimeZoneInfo.Utc
    };
}
