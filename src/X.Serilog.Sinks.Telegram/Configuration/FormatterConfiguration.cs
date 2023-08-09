namespace X.Serilog.Sinks.Telegram.Configuration;

/// <summary>
/// Represents the configuration for formatting output messages.
/// </summary>
public class FormatterConfiguration
{
    /// <summary>
    /// Gets a value indicating whether to use emojis in the output.
    /// </summary>
    public bool UseEmoji { get; init; }

    /// <summary>
    /// Gets the user-friendly name of the application.
    /// </summary>
    public string? ReadableApplicationName { get; init; }

    /// <summary>
    /// Gets a value indicating whether to include exception details in the output.
    /// </summary>
    public bool IncludeException { get; init; }

    /// <summary>
    /// Gets a value indicating whether to include property details in the output.
    /// </summary>
    public bool IncludeProperties { get; init; }
}