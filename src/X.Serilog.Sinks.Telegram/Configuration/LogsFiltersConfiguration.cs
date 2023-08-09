using System.Collections.Immutable;
using X.Serilog.Sinks.Telegram.Filters;

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
    /// Gets or initializes the operator used for combining filters. The default is 'And'.
    /// </summary>
    public LogFiltersOperator FiltersOperator { get; init; } = LogFiltersOperator.And;

    /// <summary>
    /// Gets or initializes the list of filters to be applied.
    /// </summary>
    public IImmutableList<IFilter>? Filters { get; init; }
}