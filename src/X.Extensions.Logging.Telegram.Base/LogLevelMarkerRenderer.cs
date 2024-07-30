using System;

namespace X.Extensions.Logging.Telegram.Base;

public interface ILogLevelMarkerRenderer
{
    string RenderMarker(TelegramLogLevel logLevel);
}

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
