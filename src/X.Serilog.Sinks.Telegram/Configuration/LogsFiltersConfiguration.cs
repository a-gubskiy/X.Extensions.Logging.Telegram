using X.Serilog.Sinks.Telegram.Filters.Fluent;

namespace X.Serilog.Sinks.Telegram.Configuration;

/// <summary>
/// Represents the configuration for applying logs filters.
/// </summary>
public class LogsFiltersConfiguration
{
    /// <summary>
    /// Gets or initializes a value indicating whether to apply log filters.
    /// </summary>
    public bool ApplyLogFilters { get; init; }

    /// <summary>
    /// Logs filtering query builder
    /// </summary>
    public ILogQueryBuilder? QueryBuilder { get; set; }
}