namespace X.Extensions.Serilog.Sinks.Telegram.Filters.Fluent;

public interface IFilterExecutor
{
    bool Evaluate(LogEvent entry);
}