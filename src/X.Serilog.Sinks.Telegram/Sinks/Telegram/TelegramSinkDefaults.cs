using System;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram
{
    internal static class TelegramSinkDefaults
    {
        public const int BatchPostingLimit = 20;
        public static readonly TimeSpan BatchPostingPeriod = new TimeSpan(0, 0, 5);
    }
}