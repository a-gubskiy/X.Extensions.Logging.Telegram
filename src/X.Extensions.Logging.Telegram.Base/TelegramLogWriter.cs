using System.Diagnostics;
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
        var result = await _client.SendTextMessageAsync(
            chatId: _chatId,
            text: message,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken);

        Trace.WriteLine(result.MessageId);
    }
}