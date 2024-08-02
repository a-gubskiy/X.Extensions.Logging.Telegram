using Example.Core.Helpers;
using Microsoft.Extensions.Logging;

namespace Example.Core.Services;

public class LogsService(ILogger<LogsService> logger)
{
    public void WriteLogs()
    {
        const int logsNumber = 5;

        var amount = logsNumber;
        while (amount > 0)
        {
            amount--;

            var logLevel = EnumHelpers.GetRandomValue<LogLevel>();
            logger.Log(logLevel, "Log message: " + Guid.NewGuid().ToString("N"));
        }
    }
}