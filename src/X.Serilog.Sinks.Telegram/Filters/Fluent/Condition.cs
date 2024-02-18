namespace X.Serilog.Sinks.Telegram.Filters.Fluent;

public class Condition(LogEntryPredicate predicate)
{
    public LogEntryPredicate Predicate { get; set; } = predicate;
}