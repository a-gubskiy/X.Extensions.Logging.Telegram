using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Base;
using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Logging.Telegram.Base.Formatters;
using X.Extensions.Logging.Telegram.Extensions;

namespace X.Extensions.Logging.Telegram.Tests;

public class TelegramLoggingTests
{
    public TelegramLoggingTests()
    {
        Setup();
    }

    public void Setup()
    {
    }

    [Fact]
    public void Test_DefaultLogFormatter_MessageNotNull()
    {
        var options = new TelegramLoggerOptions(LogLevel.Trace)
        {
            AccessToken = "",
            ChatId = "",
            FormatterConfiguration = new FormatterConfiguration
            {
                UseEmoji = true,
                ReadableApplicationName = "Project A"
            }
        };

        IMessageFormatter formatter = new DefaultLogFormatter(); 

        ICollection<LogEntry> logEntries = new List<LogEntry>
        {
            new LogEntry
            {
                Message = "Message1",
                Exception = null,
                Level = TelegramLogLevel.Warning,
                Properties = new Dictionary<string, string>(),
                UtcTimeStamp = DateTime.UtcNow
            }
        };
        
        var messages = formatter.Format(logEntries, options.FormatterConfiguration);

        foreach (var message in messages)
        {
            Assert.NotNull(messages);
        }
    }
    
    [Fact]
    public void Test_DefaultAggregatedNotificationsFormatter_MessageNotNull()
    {
        var options = new TelegramLoggerOptions(LogLevel.Trace)
        {
            AccessToken = "",
            ChatId = "",
            FormatterConfiguration = new FormatterConfiguration
            {
                UseEmoji = true,
                ReadableApplicationName = "Project A"
            }
        };

        IMessageFormatter formatter = new DefaultAggregatedNotificationsFormatter();

        ICollection<LogEntry> logEntries = new List<LogEntry>
        {
            new LogEntry
            {
                Message = "Message 1",
                Exception = null,
                Level = TelegramLogLevel.Error,
                Properties = new Dictionary<string, string>(),
                UtcTimeStamp = DateTime.UtcNow
            },
            new LogEntry
            {
                Message = "Message 2",
                Exception = null,
                Level = TelegramLogLevel.Information,
                Properties = new Dictionary<string, string>(),
                UtcTimeStamp = DateTime.UtcNow
            },
            new LogEntry
            {
                Message = "Message 3",
                Exception = null,
                Level = TelegramLogLevel.Warning,
                Properties = new Dictionary<string, string>(),
                UtcTimeStamp = DateTime.UtcNow
            }
        };
        
        var messages = formatter.Format(logEntries, options.FormatterConfiguration);
        var message = messages.FirstOrDefault();
    
        Assert.NotNull(message);
    }

    [Fact]
    public void Test_MessageFormatter_MessageIsNull()
    {
        var options = new TelegramLoggerOptions()
        {
            FormatterConfiguration = new FormatterConfiguration
            {
                ReadableApplicationName = "Project A",
                UseEmoji = true
            },
            AccessToken = "",
            ChatId = "",
            LogLevel = new Dictionary<string, LogLevel>
            {
                { "Default", LogLevel.Warning },
                { "Some.Namespace.SomeClassName", LogLevel.Error },
                { "Some.Namespace.AnotherClassName", LogLevel.Trace },
            }
        };

        var processor = new FakeLogQueueProcessor();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .ClearProviders()
                .AddTelegram(options, processor);
        });

        var telegramLogger0 = loggerFactory.CreateLogger("System");
        var telegramLogger1 = loggerFactory.CreateLogger("Some.Namespace.SomeClassName");
        var telegramLogger2 = loggerFactory.CreateLogger("Some.Namespace.AnotherClassName");


        telegramLogger0.LogWarning("Message from System");
        telegramLogger1.LogInformation("Message from Some.Namespace.SomeClassName");
        telegramLogger2.LogInformation("Message from Some.Namespace.AnotherClassName");


        Assert.Equal(2, processor.Messages.Count);
        
        Assert.Contains(processor.Messages, o => o.Contains("System"));
        Assert.Contains(processor.Messages, o => o.Contains("Message from Some.Namespace.AnotherClassName"));

        Assert.DoesNotContain(processor.Messages, o => o.Contains("Some.Namespace.SomeClassName"));
    }

    [Theory]
    [InlineData("<p style=\"font-family='Lucida Console'\">Exception message description</p>")]
    [InlineData("<p style=\"font-family='Lucida Console';width:100%\">Exception <br/><i><b>message</b></i> description</p>")]
    public void ExceptionDescriptionWithRawHtmlTest(string description)
    {
        IMessageFormatter formatter = new DefaultLogFormatter();

        ICollection<LogEntry> logEntries = new List<LogEntry>
        {
            new LogEntry
            {
                Message = description
            }
        };

        var configuration = new FormatterConfiguration
        {
            UseEmoji = true,
            ReadableApplicationName = "Test API"            
        };

        var messages = formatter.Format(logEntries, configuration);

        foreach (var message in messages)
        {
            var containsRawHtml = message.Contains("<p style=\"font-family='Lucida Console'\">") ||
                                  message.Contains("</p>") ||
                                  message.Contains("<br/>") ||
                                  message.Contains("<i>") ||
                                  message.Contains("</i>");

            Assert.False(containsRawHtml);
        }
    }
}