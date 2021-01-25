using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram
{
    public class TelegramLoggerOptions
    {
        public LogLevel MinimumLogLevel { get; set; } = LogLevel.Information;
        public string AccessToken { get; set; }
        public string ChatId { get; set; }
        public bool UseEmoji { get; set; } = true;
    }
}