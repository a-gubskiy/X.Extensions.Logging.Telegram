# X.Serilog.Sinks.Telegram
[![Build status](https://ci.appveyor.com/api/projects/status/n4uj9qfuywrkdrhb/branch/main?svg=true)](https://ci.appveyor.com/project/Bardin08/x-serilog-sinks-telegram/branch/main)

A Serilog sink that writes events to [Telegram](https://telegram.org/). <br/>
**NuGet Package** - [X.Serilog.Sinks.Telegram](https://www.nuget.org/packages/X.Serilog.Sinks.Telegram/)

### Configuration
In the examples below, the sink is writing to the Telegram channel. Important: Telegram bot token and chat id are absolutely required parameters! If they won't be passed, an exception will be thrown. Like other Serilog sinks, this one can be configured in two ways, by fluent configuration or by appsettings file.
```csharp
// write events to the given channel
Log.Logger = new LoggerConfiguration()
    .WriteTo.Telegram(
        token: "",
        chatId: "",
        readableApplicationName: "",
        useEmoji: true)
    .CreateLogger();
    
// manual configuration creating
Log.Logger = new LoggerConfiguration()
    .WriteTo.Telegram(config =>
    {
        config = new TelegramSinkConfiguration()
        {
            Token = "",
            ChatId = "",
            FormatterConfiguration = new FormatterConfiguration()
            {
                UseEmoji = true,
                ReadableApplicationName = "",
            },
        };
    }, messageFormatter: null!)
    .CreateLogger();
```

### JSON *(Microsoft.Extensions.Configuration)*
Keys and values are case-insensitive. This is an example of configuring Telegram sink from *appsettings.json*:

```json
"Serilog": {
  "Using": [
    "X.Serilog.Sinks.Telegram"
  ],
  "WriteTo": [
    {
      "Name": "Telegram",
      "Args": {
        "Token": "0000000000:0000_000000000000000000000000000000",
        "ChatId": "-0000000000000",
        "BatchPostingLimit": 5,
        "BatchPeriod": "0.00:00:20",
        "ReadableApplicationName": "Sample Application",
        "UseEmoji": true,
        "RestrictedToMinimumLevel": "Information",
        "Mode": "Logs"
      }
    }
  ],
  "Properties": {
    "Application": "Human readable application name"
  }
}
```

### Settings description
 - Token — Telegram bot token.
 - ChatId — Telegram channel/chat id. Identifies the channel where events will be written.
 - BatchPostingLimit — The minimum amount of events that should be collected before they will be written.
 - BatchPeriod — The minimum time which should pass between events of writing.
 - FormatterConfiguration — Message formatter configuration. Requires for passing additional info or a formatter as is.
 - Mode — [Events representing mode](https://github.com/Bardin08/X.Serilog.Sinks.Telegram/new/main?readme=1#events-representing-modes).
 - UseEmoji — This allows replacing text log level representing with an emoji.
 - ReadableApplicationName — Text from this field will be used as a human-readable application name.

### Events representing modes
Now supported 3 modes of message formatting: logs, notifications and aggregated notifications.
 - Logs — Represents a common log message with a great amount of additional information.
 - Notifications — Represents a short message without any details.
 - Aggregated notifications —  Represents a bench of short messages without any details.
