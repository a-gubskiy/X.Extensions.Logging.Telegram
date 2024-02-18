using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Filters.Fluent;

namespace X.Serilog.Sinks.Telegram.Filters;

public class LogFilterManager(LogsFiltersConfiguration configuration)
{
    private readonly IFilterExecutor _logsFilter = (IFilterExecutor)configuration.QueryBuilder!;

    public bool ApplyFilter(LogEvent logEvent)
    {
        return _logsFilter.Evaluate(logEvent);
    }
}