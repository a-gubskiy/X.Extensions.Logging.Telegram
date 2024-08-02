using System;
using System.Threading.Tasks;
using Example.Core;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram;
using X.Extensions.Logging.Telegram.Extensions;

namespace Example.ConsoleApp;

class Program
{
    public class ExampleClass
    {
    }
        
    public class AnotherExampleClass
    {
    }
        
    static void Main(string[] args)
    {
        var options = new TelegramLoggerOptions(LogLevel.Information)
        {
            AccessToken = ExampleAppSettings.Token,
            ChatId = ExampleAppSettings.ChatId,
            Source = "TEST APP",
            UseEmoji = true
        };
            
        var factory = LoggerFactory.Create(builder =>
            {
                builder
                    .ClearProviders()
                    .AddTelegram(options)
                    .AddConsole();
            }
        );

        var logger1 = factory.CreateLogger<ExampleClass>();
        var logger2 = factory.CreateLogger<AnotherExampleClass>();

        for (var i = 0; i < 1; i++)
        {
            logger1.LogTrace($"Message {i}");
            logger2.LogDebug($"Debug message text {i}");
            logger1.LogInformation($"Information message text {i}");

            try
            {
                throw new SystemException("Exception message description. <br /> This message contains " +
                                          "<html> <tags /> And some **special** symbols _");
            }
            catch (Exception exception)
            {
                logger2.LogWarning(exception, $"Warning message text {i}");
                logger1.LogError(exception, $"Error message  text {i}");
                logger2.LogCritical(exception, $"Critical error message  text {i}");    
            }

            Task.WaitAll(Task.Delay(500));
        }


        Console.WriteLine("Hello World!");
        Console.ReadKey();
    }
}