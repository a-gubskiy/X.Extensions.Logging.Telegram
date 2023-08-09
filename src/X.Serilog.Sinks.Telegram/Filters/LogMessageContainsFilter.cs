namespace X.Serilog.Sinks.Telegram.Filters;

public class LogMessageContainsFilter : IFilter
{
    private readonly string _value;

    public LogMessageContainsFilter(string value)
    {
        _value = value;
    }

    public bool IsPassedAsync(LogEvent logEvent)
    {
        var isPassed = logEvent.RenderMessage().Contains(_value, StringComparison.InvariantCultureIgnoreCase);
        return isPassed;
    }
}