using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace X.Extensions.Logging.Telegram.Tests
{
    public class TelegramLoggingTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            var options = new TelegramLoggerOptions
            {
                Source = "Project A",
                AccessToken = "",
                ChatId = "",
                LogLevel = LogLevel.Trace,
                UseEmoji = true
            };
            
            var formatter = new TelegramMessageFormatter(options, "Some.Namespace.SomeClassName");
            var message = formatter.Format(LogLevel.Warning, new EventId(), "Message", null, (s, _) => s);

            Assert.NotNull(message);
        }
    }
}