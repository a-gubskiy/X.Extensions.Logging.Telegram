namespace X.Serilog.Sinks.Telegram.Filters;

public class LogMessageNotContainsFilter : IFilter
{
    private readonly string _value;

    public LogMessageNotContainsFilter(string value)
    {
        _value = value;
    }

    public bool IsPassedAsync(LogEvent logEvent)
    {
        var isContains = logEvent.RenderMessage().Contains(_value, StringComparison.InvariantCultureIgnoreCase);
        return !isContains;
    }
}