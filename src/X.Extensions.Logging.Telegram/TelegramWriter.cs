using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace X.Extensions.Logging.Telegram
{
    public class TelegramWriter
    {
        private readonly string _chatId;
        private readonly TelegramBotClient _client;

        public TelegramWriter(string accessToken, string chatId)
        {
            _chatId = chatId;
            _client = new TelegramBotClient(accessToken);
        }

        public void Write(string message)
        {
            var task = _client.SendTextMessageAsync(_chatId, message, ParseMode.Markdown);
            
            Task.WaitAll(task);
        }
    }
}