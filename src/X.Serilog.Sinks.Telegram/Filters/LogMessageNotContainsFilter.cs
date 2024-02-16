namespace X.Serilog.Sinks.Telegram.Filters;

public class LogMessageNotContainsFilter(string value) : IFilter
{
    public bool IsPassedAsync(LogEvent logEvent) =>
        !logEvent.RenderMessage().Contains(value, StringComparison.InvariantCultureIgnoreCase);
}