using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Base.Configuration;

namespace X.Extensions.Logging.Telegram;

[PublicAPI]
public record TelegramLoggerOptions
{
    public Dictionary<string, LogLevel> LogLevel { get; set; }
    
    public string AccessToken { get; set; } = "";
    
    public string ChatId { get; set; } = "";
    
    public FormatterConfiguration FormatterConfiguration { get; set; } = FormatterConfiguration.Default;
    
    public TelegramLoggerOptions()
        : this(Microsoft.Extensions.Logging.LogLevel.Information)
    {
    }

    public TelegramLoggerOptions(LogLevel logLevel)
    {
        LogLevel = new() { { "Default", logLevel } };
    }
}