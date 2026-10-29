using Example.Core;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram;
using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Logging.Telegram.Extensions;

var options = new TelegramLoggerOptions(LogLevel.Information)
{
    AccessToken = ExampleAppSettings.Token,
    ChatId = ExampleAppSettings.ChatId,
    FormatterConfiguration = new FormatterConfiguration
    {
        ReadableApplicationName = "TEST APP",
        UseEmoji = true,
        IncludeException = true,
        IncludeProperties = true
    }
};

using (var factory = LoggerFactory.Create(builder => builder
           .ClearProviders()
           .AddTelegram(options)
           .AddConsole()))
{
    var logger = factory.CreateLogger("Example.ConsoleApp");

    logger.LogTrace("Trace message");
    logger.LogDebug("Debug message");
    logger.LogInformation("Information message");

    try
    {
        throw new SystemException("Exception message description. <br /> This message contains " +
                                  "<html> <tags /> And some **special** symbols _");
    }
    catch (Exception exception)
    {
        logger.LogWarning(exception, "Warning message");
        logger.LogError(exception, "Error message");
        logger.LogCritical(exception, "Critical error message");
    }
}

Console.WriteLine("Done.");
