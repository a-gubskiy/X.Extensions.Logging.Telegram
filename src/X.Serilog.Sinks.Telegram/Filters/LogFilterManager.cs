using System.Collections.Immutable;
using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Filters;

public class LogFilterManager
{
    private readonly LogsFiltersConfiguration _configuration;
    private readonly IImmutableList<IFilter> _logsFilters;

    public LogFilterManager(LogsFiltersConfiguration configuration)
    {
        _configuration = configuration;
        _logsFilters = configuration.Filters ?? ImmutableArray<IFilter>.Empty;
    }

    public bool ApplyFilter(LogEvent logEvent)
    {
        var filterResults = _logsFilters
            .Select(filter => filter.IsPassedAsync(logEvent));

        var isPassed = _configuration.FiltersOperator switch
        {
            LogFiltersOperator.And => filterResults.All(result => result),
            LogFiltersOperator.Or => filterResults.Any(result => result),
            _ => throw new ArgumentOutOfRangeException()
        };

        return isPassed;
    }
}