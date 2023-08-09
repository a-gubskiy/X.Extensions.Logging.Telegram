using X.Serilog.Sinks.Telegram.Formatters;

namespace X.Serilog.Sinks.Telegram.Configuration;

public static class TelegramSinkDefaults
{
    public const int BatchPostingLimit = 20;
    public static readonly TimeSpan BatchPostingPeriod = TimeSpan.FromSeconds(20);
    public static readonly TimeSpan RulesCheckPeriod = TimeSpan.FromSeconds(5);

    internal static IMessageFormatter GetDefaultMessageFormatter(LoggingMode loggingMode)
    {
        return loggingMode switch
        {
            LoggingMode.Logs => new DefaultLogFormatter(),
            LoggingMode.AggregatedNotifications => new DefaultAggregatedNotificationsFormatter(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}