using X.Serilog.Sinks.Telegram.Formatters;

namespace X.Serilog.Sinks.Telegram.Configuration;

internal static class TelegramSinkDefaults
{
    public const int BatchPostingLimit = 20;
    public static readonly TimeSpan BatchPostingPeriod = new(0, 0, 20);

    internal static IMessageFormatter GetDefaultMessageFormatter(LoggingMode loggingMode)
    {
        return loggingMode switch
        {
            LoggingMode.Logs => new DefaultLogFormatter(),
            LoggingMode.Notifications => new DefaultNotificationFormatter(),
            LoggingMode.AggregatedNotifications => new DefaultAggregatedNotificationsFormatter(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}