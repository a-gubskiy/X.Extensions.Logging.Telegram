using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using X.Extensions.Logging.Telegram.Base;
using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Logging.Telegram.Base.Formatters;
using X.Extensions.Logging.Telegram.Extensions;

namespace X.Extensions.Logging.Telegram.Tests;

public class TelegramLoggingTests
{
    [Fact]
    public void DefaultLogFormatter_FormatsSingleEntry()
    {
        var configuration = new FormatterConfiguration
        {
            UseEmoji = true,
            ReadableApplicationName = "Project A"
        };

        IMessageFormatter formatter = new DefaultLogFormatter();

        var messages = formatter.Format(
        [
            new LogEntry
            {
                Message = "Message1",
                Level = TelegramLogLevel.Warning,
                UtcTimeStamp = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc)
            }
        ], configuration);

        var message = Assert.Single(messages);

        Assert.Contains("Warning", message);
        Assert.Contains("[Project A]", message);
        Assert.Contains("[2026-01-02 03:04:05 UTC]", message);
        Assert.Contains("<code>Message1</code>", message);
    }

    [Fact]
    public void DefaultLogFormatter_FormatsEachEntrySeparately()
    {
        IMessageFormatter formatter = new DefaultLogFormatter();

        var messages = formatter.Format(
        [
            new LogEntry { Message = "Message 1", Level = TelegramLogLevel.Error },
            new LogEntry { Message = "Message 2", Level = TelegramLogLevel.Information }
        ], FormatterConfiguration.Default);

        Assert.Equal(2, messages.Count);
    }

    [Fact]
    public void DefaultLogFormatter_EncodesException()
    {
        var configuration = new FormatterConfiguration { IncludeException = true };

        IMessageFormatter formatter = new DefaultLogFormatter();

        var message = Assert.Single(formatter.Format(
        [
            new LogEntry { Message = "Failure", Exception = "System.Exception: <b>boom</b>" }
        ], configuration));

        Assert.Contains("<b>Exception:</b> <code>System.Exception: &lt;b&gt;boom&lt;/b&gt;</code>", message);
    }

    [Fact]
    public void Logger_RespectsCategoryLogLevels()
    {
        var options = new TelegramLoggerOptions
        {
            FormatterConfiguration = new FormatterConfiguration
            {
                ReadableApplicationName = "Project A",
                UseEmoji = true
            },
            LogLevel = new Dictionary<string, LogLevel>
            {
                { "Default", LogLevel.Warning },
                { "Some.Namespace.SomeClassName", LogLevel.Error },
                { "Some.Namespace.AnotherClassName", LogLevel.Trace },
            }
        };

        var processor = new FakeLogQueueProcessor();

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .ClearProviders()
                .AddTelegram(options, processor);
        });

        loggerFactory.CreateLogger("System").LogWarning("Message from System");
        loggerFactory.CreateLogger("Some.Namespace.SomeClassName").LogInformation("Message from Some.Namespace.SomeClassName");
        loggerFactory.CreateLogger("Some.Namespace.AnotherClassName").LogInformation("Message from Some.Namespace.AnotherClassName");

        Assert.Equal(2, processor.Messages.Count);
        Assert.Contains(processor.Messages, o => o.Contains("Message from System"));
        Assert.Contains(processor.Messages, o => o.Contains("Message from Some.Namespace.AnotherClassName"));
        Assert.DoesNotContain(processor.Messages, o => o.Contains("Some.Namespace.SomeClassName"));
    }

    [Fact]
    public void Logger_DeliversQueuedMessagesOnDispose()
    {
        var writer = new FakeLogWriter();

        using (var loggerFactory = LoggerFactory.Create(builder => builder
                   .ClearProviders()
                   .AddTelegram(new TelegramLoggerOptions(LogLevel.Information), writer)))
        {
            var logger = loggerFactory.CreateLogger("Test");

            for (var i = 0; i < 5; i++)
            {
                logger.LogInformation("Message {Index}", i);
            }
        }

        Assert.Equal(5, writer.Messages.Count);
    }

    [Fact]
    public void Logger_ReadsOptionsFromConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Logging:Telegram:AccessToken"] = "123:token",
                ["Logging:Telegram:ChatId"] = "42",
                ["Logging:Telegram:FormatterConfiguration:ReadableApplicationName"] = "Configured App",
                ["Logging:Telegram:FormatterConfiguration:IncludeException"] = "true"
            })
            .Build();

        var services = new ServiceCollection()
            .AddLogging(builder => builder.AddTelegram(configuration))
            .BuildServiceProvider();

        var logger = Assert.IsType<TelegramLogger>(services.GetRequiredService<ILoggerProvider>().CreateLogger("Test"));

        Assert.Equal("123:token", logger.Options.AccessToken);
        Assert.Equal("42", logger.Options.ChatId);
        Assert.Equal("Configured App", logger.Options.FormatterConfiguration.ReadableApplicationName);
        Assert.True(logger.Options.FormatterConfiguration.IncludeException);
    }

    [Theory]
    [InlineData("<p style=\"font-family='Lucida Console'\">Exception message description</p>")]
    [InlineData("<p style=\"font-family='Lucida Console';width:100%\">Exception <br/><i><b>message</b></i> description</p>")]
    public void DefaultLogFormatter_EncodesRawHtml(string description)
    {
        IMessageFormatter formatter = new DefaultLogFormatter();

        var configuration = new FormatterConfiguration
        {
            UseEmoji = true,
            ReadableApplicationName = "Test API"
        };

        var message = Assert.Single(formatter.Format([new LogEntry { Message = description }], configuration));

        Assert.DoesNotContain("<p", message);
        Assert.DoesNotContain("</p>", message);
        Assert.DoesNotContain("<br/>", message);
        Assert.DoesNotContain("<i>", message);
        Assert.DoesNotContain("</i>", message);
    }
}
