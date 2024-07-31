namespace X.Extensions.Serilog.Sinks.Telegram.Filters.Fluent;

public class ConditionGroup(bool isOrGroup = false)
{
    public List<object> Conditions { get; set; } = [];
    public bool IsOrGroup { get; set; } = isOrGroup;
}