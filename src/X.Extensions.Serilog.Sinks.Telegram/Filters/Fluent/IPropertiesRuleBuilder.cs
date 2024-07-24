namespace X.Extensions.Serilog.Sinks.Telegram.Filters.Fluent;

public interface IPropertiesRuleBuilder : ILogQueryBuilder
{
    ILogQueryBuilder Contains(string substring);
    ILogQueryBuilder NotContains(string substring);
    ILogQueryBuilder Equals(string key, string value, StringComparison comparison);
    ILogQueryBuilder NotEquals(string key, string value, StringComparison comparison);
    ILogQueryBuilder Null();
    ILogQueryBuilder NotNull();
}