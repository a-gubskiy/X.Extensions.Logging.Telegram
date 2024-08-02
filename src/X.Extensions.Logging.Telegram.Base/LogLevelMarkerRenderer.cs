using System;

namespace X.Extensions.Logging.Telegram.Base;

public interface ILogLevelMarkerRenderer
{
    string RenderMarker(TelegramLogLevel logLevel);
}

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

/// <summary>
/// Render log level as text symbols
/// </summary>
public class LogLevelTextMarkerRenderer : ILogLevelMarkerRenderer
{
    public string RenderMarker(TelegramLogLevel level)
    {
        var result = level switch
        {
            TelegramLogLevel.Trace => "VRB",
            TelegramLogLevel.Debug => "DBG",
            TelegramLogLevel.Information => "INF",
            TelegramLogLevel.Warning => "WARN",
            TelegramLogLevel.Error => "ERR",
            TelegramLogLevel.Critical => "FTL",
            TelegramLogLevel.None => " ",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        return result;
    }
}
