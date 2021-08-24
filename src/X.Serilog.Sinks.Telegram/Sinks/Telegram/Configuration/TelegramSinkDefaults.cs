using System;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram.Configuration
{
    internal static class TelegramSinkDefaults
    {
        public const int BatchPostingLimit = 20;
        public static readonly TimeSpan BatchPostingPeriod = new TimeSpan(0, 0, 20);
    }
}