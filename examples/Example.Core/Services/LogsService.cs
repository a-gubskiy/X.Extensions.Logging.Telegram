using Example.Core.Helpers;
using Microsoft.Extensions.Logging;

namespace Example.Core.Services;

public class LogsService(ILogger<LogsService> logger)
{
    private const int LogsNumber = 5;

    public void WriteLogs()
    {
        for (var i = 0; i < LogsNumber; i++)
        {
            var logLevel = EnumHelpers.GetRandomValue<LogLevel>();
            logger.Log(logLevel, "Log message: {Id}", Guid.NewGuid().ToString("N"));
        }
    }
}
