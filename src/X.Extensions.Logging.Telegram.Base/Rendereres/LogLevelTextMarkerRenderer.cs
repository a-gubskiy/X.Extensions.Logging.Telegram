using System;

namespace X.Extensions.Logging.Telegram.Base.Rendereres;

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
