# X.Extensions.Logging.Telegram

[![NuGet](https://img.shields.io/nuget/v/X.Extensions.Logging.Telegram)](https://www.nuget.org/packages/X.Extensions.Logging.Telegram)
[![NuGet Downloads](https://img.shields.io/nuget/dt/X.Extensions.Logging.Telegram)](https://www.nuget.org/packages/X.Extensions.Logging.Telegram)
[![Sponsor on GitHub](https://img.shields.io/badge/Sponsor_on_GitHub-ff7f00?logo=github&logoColor=white&style=for-the-badge)](https://github.com/sponsors/a-gubskiy)
[![Subscribe on X](https://img.shields.io/badge/Subscribe_on_X-000000?logo=x&logoColor=white&style=for-the-badge)](https://x.com/andrew_gubskiy)

Telegram logging provider for `Microsoft.Extensions.Logging`. Receive important log messages directly on your phone or laptop.

```
dotnet add package X.Extensions.Logging.Telegram
```

Targets `netstandard2.0` and `net10.0`.

## Prepare Telegram bot and channel

1. [Create a Telegram bot](https://core.telegram.org/bots#how-do-i-create-a-bot) and copy its access token.
2. Add the bot to your channel or chat as an admin with permission to post messages.
3. Get the chat id. For a public channel you can use its name (`@channel_name`). For a private channel, forward any message from it to [@JsonDumpBot](https://t.me/jsondumpbot) to get the id.

## Configure in code

```csharp
var options = new TelegramLoggerOptions(LogLevel.Information)
{
    AccessToken = "1234567890:AAAaaAAaa_AaAAaa-AAaAAAaAAaAaAaAAAA",
    ChatId = "-0000000000000",
    FormatterConfiguration = new FormatterConfiguration
    {
        ReadableApplicationName = "Human Readable Project Name",
        UseEmoji = true,
        IncludeException = true
    }
};

builder.Logging
    .ClearProviders()
    .AddTelegram(options)
    .AddConsole();
```

Or with a delegate:

```csharp
builder.Logging.AddTelegram(options =>
{
    options.AccessToken = "...";
    options.ChatId = "...";
    options.FormatterConfiguration.ReadableApplicationName = "My App";
});
```

## Configure with appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    },
    "Telegram": {
      "LogLevel": {
        "Default": "Error",
        "WebApp.Controllers": "Warning"
      },
      "AccessToken": "1234567890:AAAaaAAaa_AaAAaa-AAaAAAaAAaAaAaAAAA",
      "ChatId": "1234567890",
      "FormatterConfiguration": {
        "ReadableApplicationName": "Human Readable Project Name",
        "UseEmoji": true,
        "IncludeException": true
      }
    }
  }
}
```

and pass `IConfiguration` to the extension method:

```csharp
builder.Logging.AddTelegram(builder.Configuration);
```

## Formatter options

| Option                    | Description                                         |
|---------------------------|-----------------------------------------------------|
| `ReadableApplicationName` | Application name shown in every message.            |
| `UseEmoji`                | Prefix the log level with an emoji.                 |
| `IncludeException`        | Include the exception details.                      |
| `IncludeProperties`       | Include log entry properties.                       |
| `TimeZone`                | Time zone for timestamps. UTC by default.           |

## Custom log writer

Implement `ILogWriter` to deliver messages your own way:

```csharp
builder.Logging.AddTelegram(options, new CustomLogWriter());
```

## Custom message formatter

Implement `IMessageFormatter` (or derive from `MessageFormatterBase`) and pass a factory. The factory receives the logger category name:

```csharp
builder.Logging.AddTelegram(options, category => new CustomMessageFormatter(category));
```

## Examples

The [examples](https://github.com/a-gubskiy/X.Extensions.Logging.Telegram/tree/main/examples) folder contains a console app and a web app.

## Contributing

Feel free to add any improvements you want via pull requests. All pull requests must be linked to an issue.

## License

This project is licensed under the MIT License.
