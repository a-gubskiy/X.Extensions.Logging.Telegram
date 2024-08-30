using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;

namespace X.Extensions.Serilog.Sinks.Telegram.Configuration;

/// <summary>
/// Configuration settings for the Telegram Sink.
/// </summary>
public class TelegramSinkConfiguration
{
    private int _batchPostingLimit = TelegramSinkDefaults.BatchPostingLimit;
    private string _chatId = null!;
    private string _token = null!;

    /// <summary>
    /// Initializes a new instance of the TelegramSinkConfiguration class.
    /// </summary>
    /// <param name="logsAccessor">The logs accessor.</param>
    public TelegramSinkConfiguration(ILogsQueueAccessor logsAccessor)
    {
        LogsAccessor = logsAccessor;
    }

    /// <summary>
    /// Gets the logs accessor.
    /// </summary>
    public ILogsQueueAccessor LogsAccessor { get; }

    /// <summary>
    /// Gets or initializes token for Telegram API access.
    /// </summary>
    public string Token
    {
        get => _token;
        set
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Invalid token! Token must be not null, empty or whitespace!");
            }

            _token = value;
        }
    }

    /// <summary>
    /// Gets or initializes token for Telegram API access.
    /// </summary>
    public string ChatId
    {
        get => _chatId;
        set
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || !long.TryParse(value, out _))
            {
                throw new ArgumentException("Invalid chat id! It must be not null, empty or whitespace " +
                                            "and it's should be a number!");
            }

            _chatId = value;
        }
    }

    /// <summary>
    /// Gets or initializes batch posting limit. This limit is the maximum number of events to post in a single batch.
    /// </summary>
    public int BatchPostingLimit
    {
        get => _batchPostingLimit;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Invalid batch posting limit! It must be greater than 0!");
            }

            _batchPostingLimit = value;
        }
    }

    /// <summary>
    /// Gets or sets the logging mode.
    /// </summary>
    public LoggingMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the configuration used for formatting the logs.
    /// </summary>
    public FormatterConfiguration FormatterConfiguration { get; set; } = null!;

    /// <summary>
    /// Gets or sets the configuration for rules on emitting batches.
    /// </summary>
    public BatchEmittingRulesConfiguration BatchEmittingRulesConfiguration { get; set; } = new();

    /// <summary>
    /// Gets or sets the configuration for filtering logs.
    /// </summary>
    public LogsFiltersConfiguration? LogFiltersConfiguration { get; set; }

    /// <summary>
    /// Validates the current configuration.
    /// If validation fails, it throws an exception.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrEmpty(Token) || string.IsNullOrWhiteSpace(Token))
        {
            throw new ArgumentException("Invalid token! Token must be not null, empty or whitespace!");
        }

        if (string.IsNullOrEmpty(ChatId) || string.IsNullOrWhiteSpace(ChatId) || !long.TryParse(ChatId, out _))
        {
            throw new ArgumentException("Invalid chat id! It must be not null, empty or whitespace " +
                                        "and it's should be a number!");
        }

        if (BatchPostingLimit <= 0)
        {
            throw new ArgumentException("Invalid batch posting limit! It must be greater than 0!");
        }

        if (FormatterConfiguration is null)
        {
            throw new ArgumentNullException(nameof(FormatterConfiguration));
        }
    }
}