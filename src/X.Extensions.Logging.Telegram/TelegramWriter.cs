using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace X.Extensions.Logging.Telegram
{
    public interface ITelegramWriter
    {
        Task Write(string message);
    }

    public class TelegramWriter : ITelegramWriter
    {
        private readonly string _chatId;
        private readonly TelegramBotClient _client;

        public TelegramWriter(string accessToken, string chatId)
        {
            _chatId = chatId;
            _client = new TelegramBotClient(accessToken);
        }

        public async Task Write(string message) =>
            await _client.SendTextMessageAsync(_chatId, message, ParseMode.Markdown);
    }
}