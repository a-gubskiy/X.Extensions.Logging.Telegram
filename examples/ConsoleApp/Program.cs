using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram;

namespace ConsoleApp
{
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
            var options = new TelegramLoggerOptions
            {
                AccessToken = "xxx",
                ChatId = "-0000000000000",
                MinimumLogLevel = LogLevel.Information
            };
            
            var factory = LoggerFactory.Create(builder =>
                {
                    builder
                        .ClearProviders()
                        .AddTelegram(options)
                        .AddConsole();;
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
                    throw new SystemException("Exception message description");

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
}