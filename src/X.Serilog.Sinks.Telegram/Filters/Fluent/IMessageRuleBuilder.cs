namespace X.Serilog.Sinks.Telegram.Filters.Fluent;

public interface IMessageRuleBuilder : ILogQueryBuilder
{
    ILogQueryBuilder Contains(string substring);
    ILogQueryBuilder NotContains(string substring);
    ILogQueryBuilder Equals(string message, StringComparison comparison);
    ILogQueryBuilder NotEquals(string message, StringComparison comparison);
    ILogQueryBuilder Null();
    ILogQueryBuilder NotNull();
}