namespace X.Serilog.Sinks.Telegram.Filters;

public class LogMessageContainsFilter(string value) : IFilter
{
    public bool IsPassedAsync(LogEvent logEvent) =>
        logEvent.RenderMessage().Contains(value, StringComparison.InvariantCultureIgnoreCase);
}