using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace X.Extensions.Logging.Telegram.Base;

[PublicAPI]
public interface ILogWriter
{
    Task Write(string message);

    Task Write(string message, CancellationToken cancellationToken);
}

public class TelegramLogWriter : ILogWriter
{
    private const int RetryCount = 5;

    private readonly string _chatId;
    private readonly ITelegramBotClient _client;

    public TelegramLogWriter(string accessToken, string chatId)
        : this(new TelegramBotClient(accessToken), chatId)
    {
    }

    public TelegramLogWriter(ITelegramBotClient client, string chatId)
    {
        _chatId = chatId;
        _client = client;
    }

    public Task Write(string message) => Write(message, CancellationToken.None);

    public async Task Write(string message, CancellationToken cancellationToken)
    {
        for (var attempt = 0; ; attempt++)
        {
            try
            {
                await _client.SendMessage(
                    chatId: _chatId,
                    text: message,
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);

                return;
            }
            catch (Exception) when (attempt < RetryCount && !cancellationToken.IsCancellationRequested)
            {
                // Exponential backoff: 1s, 2s, 4s, 8s, 16s
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)), cancellationToken);
            }
        }
    }
}
