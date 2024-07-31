namespace X.Extensions.Serilog.Sinks.Telegram.Filters.Fluent;

public interface ILevelRuleBuilder : ILogQueryBuilder
{
    ILogQueryBuilder InRange(LogEventLevel min, LogEventLevel max);
    ILogQueryBuilder NotInRange(LogEventLevel min, LogEventLevel max);
    ILogQueryBuilder Equals(LogEventLevel level);
    ILogQueryBuilder NotEquals(LogEventLevel level);
}