using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Polly;
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

    public async Task Write(string message)
    {
        await Write(message, CancellationToken.None);
    }

    public async Task Write(string message, CancellationToken cancellationToken)
    {
        const int retryCount = 5;

        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: retryCount,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, context) =>
                {
                    // ignored
                });

        await retryPolicy.ExecuteAsync(async () =>
        {
            var result = await _client.SendTextMessageAsync(
                chatId: _chatId,
                text: message,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        });
    }
}