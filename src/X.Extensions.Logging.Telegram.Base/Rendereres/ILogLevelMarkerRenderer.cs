namespace X.Extensions.Logging.Telegram.Base.Rendereres;

public interface ILogLevelMarkerRenderer
{
    string RenderMarker(TelegramLogLevel logLevel);
}
