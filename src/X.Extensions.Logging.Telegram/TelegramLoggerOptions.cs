using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram;

[PublicAPI]
public class TelegramLoggerOptions
{
    public Dictionary<string, LogLevel> LogLevel { get; set; }
    public string AccessToken { get; set; }
    public string ChatId { get; set; }
    public bool UseEmoji { get; set; }
    public string Source { get; set; }
    
    public TelegramLoggerOptions()
        : this(Microsoft.Extensions.Logging.LogLevel.Information)
    {
    }

    public TelegramLoggerOptions(LogLevel defaultLogLevel)
    {
        UseEmoji = true;
        LogLevel = new() { { "Default", defaultLogLevel } };
    }
}