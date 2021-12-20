using System.Collections.Generic;
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
        public void Test_MessageFormatter_MessageNotNull()
        {
            var options = new TelegramLoggerOptions(LogLevel.Trace)
            {
                Source = "Project A",
                AccessToken = "",
                ChatId = "",
                UseEmoji = true
            };
            
            var formatter = new TelegramMessageFormatter(options, "Some.Namespace.SomeClassName");
            var message = formatter.Format(LogLevel.Warning, new EventId(), "Message", null, (s, _) => s);

            Assert.NotNull(message);
        }

        [TestCase("<p style=\"font-family='Lucida Console'\">Exception message description</p>")]
        [TestCase("<p style=\"font-family='Lucida Console';width:100%\">Exception <br/><i><b>message</b></i> description</p>")]
        public void ExceptionDescriptionWithRawHtmlTest(string description)
        {
            ITelegramMessageFormatter formatter = new TelegramMessageFormatter(new TelegramLoggerOptions()
            {
                Source = "Test API",
                AccessToken = "none",
                ChatId = "12345",
                UseEmoji = true,
                LogLevel = new Dictionary<string, LogLevel>
                {
                    {"test", LogLevel.Information}
                }
            }, "test");

            var result = formatter.EncodeHtml(description);
            
            var containsRawHtml = result.Contains("<p style=\"font-family='Lucida Console'\">") ||
                                  result.Contains("</p>") ||
                                  result.Contains("<br/>") ||
                                  result.Contains("<i>") ||
                                  result.Contains("</i>") ||
                                  result.Contains("<b>") ||
                                  result.Contains("</b>");

            Assert.False(containsRawHtml);
        }
    }
}