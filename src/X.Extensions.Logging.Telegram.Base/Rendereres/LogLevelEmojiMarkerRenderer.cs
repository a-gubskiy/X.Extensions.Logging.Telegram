using System;

namespace X.Extensions.Logging.Telegram.Base.Rendereres;

/// <summary>
/// Render log level marker as emoji
/// </summary>
public class LogLevelEmojiMarkerRenderer : ILogLevelMarkerRenderer
{
    public string RenderMarker(TelegramLogLevel level)
    {
        var result =  level switch
        {
            TelegramLogLevel.Trace => "📝",
            TelegramLogLevel.Debug => "📓",
            TelegramLogLevel.Information => "ℹ️",
            TelegramLogLevel.Warning => "⚠️",
            TelegramLogLevel.Error => "❗",
            TelegramLogLevel.Critical => "❌",
            TelegramLogLevel.None => "☠️️",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        return result;
    }
}