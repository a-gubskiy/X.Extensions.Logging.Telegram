namespace X.Serilog.Sinks.Telegram.Filters;

public interface IFilter
{
    bool IsPassedAsync(LogEvent logEvent);
}