using System.Net;
using System.Text.Encodings.Web;

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

        [TestCase("<p style=\"font-family='Lucida Console'\">Exception message description</p>")]
        [TestCase("<p style=\"font-family='Lucida Console';width:100%\">Exception <br/><i><b>message</b></i> description</p>")]
        public void ExceptionDescriptionWithRawHtmlTest(string description)
        {
            var encodedHtml = WebUtility.HtmlEncode(description);

            var containsRawHtml = encodedHtml.Contains("<p style=\"font-family='Lucida Console'\">") ||
                                        encodedHtml.Contains("</p>") ||
                                        encodedHtml.Contains("<br/>") ||
                                        encodedHtml.Contains("<i>") ||
                                        encodedHtml.Contains("</i>") ||
                                        encodedHtml.Contains("<b>") ||
                                        encodedHtml.Contains("</b>");

            Assert.False(containsRawHtml);
        }
    }
}