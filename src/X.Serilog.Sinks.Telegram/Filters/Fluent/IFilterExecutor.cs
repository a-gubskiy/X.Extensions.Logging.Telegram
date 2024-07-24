namespace X.Serilog.Sinks.Telegram.Filters.Fluent;

public interface IFilterExecutor
{
    bool Evaluate(LogEvent entry);
}