using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace X.Extensions.Logging.Telegram;

[PublicAPI]
public interface ITelegramWriter
{
    Task Write(string message);
}

public class TelegramWriter : ITelegramWriter
{
    private readonly string _chatId;
    private readonly ITelegramBotClient _client;

    public TelegramWriter(string accessToken, string chatId)
        : this(new TelegramBotClient(accessToken), chatId)
    {
    }

    public TelegramWriter(ITelegramBotClient client, string chatId)
    {
        _chatId = chatId;
        _client = client;
    }

    public async Task Write(string message) =>
        await _client.SendTextMessageAsync(_chatId, message, ParseMode.Html);
}