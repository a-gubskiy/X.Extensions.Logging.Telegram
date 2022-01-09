namespace X.Serilog.Sinks.Telegram.Configuration;

internal static class TelegramSinkDefaults
{
    public const int BatchPostingLimit = 20;
    public static readonly TimeSpan BatchPostingPeriod = new(0, 0, 20);
}