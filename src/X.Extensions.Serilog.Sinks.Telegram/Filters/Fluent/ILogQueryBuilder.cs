namespace X.Extensions.Serilog.Sinks.Telegram.Filters.Fluent;

public interface ILogQueryBuilder
{
    IMessageRuleBuilder Message { get; }
    IExceptionRuleBuilder Exception { get; }
    IPropertiesRuleBuilder Properties { get; }
    ILevelRuleBuilder Level { get; }
    ILogQueryBuilder And();
    ILogQueryBuilder Or();
}