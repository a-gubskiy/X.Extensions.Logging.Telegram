using System;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram;

public interface ILogLevelMarkerRenderer
{
    string RenderMarker(LogLevel logLevel);
}

public class LogLevelEmojiMarkerRenderer : ILogLevelMarkerRenderer
{
    public string RenderMarker(LogLevel level)
    {
        var result =  level switch
        {
            LogLevel.Trace => "📝",
            LogLevel.Debug => "📓",
            LogLevel.Information => "ℹ️",
            LogLevel.Warning => "⚠️",
            LogLevel.Error => "❗",
            LogLevel.Critical => "❌",
            LogLevel.None => "☠️️",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        return result;
    }
}

public class LogLevelTextMarkerRenderer : ILogLevelMarkerRenderer
{
    public string RenderMarker(LogLevel level)
    {
        var result = level switch
        {
            LogLevel.Trace => "VRB",
            LogLevel.Debug => "DBG",
            LogLevel.Information => "INF",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERR",
            LogLevel.Critical => "FTL",
            LogLevel.None => " ",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };

        return result;
    }
}
