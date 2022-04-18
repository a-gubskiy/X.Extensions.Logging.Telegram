# X.Serilog.Sinks.Telegram
X.Serilog.Sinks.Telegram is a Serilog sink that write events to [Telegram](https://telegram.org/) channel or chat.

## Current Statuses
[![Build status](https://ci.appveyor.com/api/projects/status/n4uj9qfuywrkdrhb/branch/main?svg=true)](https://ci.appveyor.com/project/Bardin08/x-serilog-sinks-telegram/branch/main)
[![NuGet Badge](https://buildstats.info/nuget/X.Serilog.Sinks.Telegram)](https://www.nuget.org/packages/X.Serilog.Sinks.Telegram/)

## Installation

The package can be installed by NuGet manually or within one of the listed commands.

```ps
PM> Install-Package X.Serilog.Sinks.Telegram -Version 1.0.0
```

```sh
$ dotnet add package X.Serilog.Sinks.Telegram --version 1.0.0
```

## Usage

Check [docs](./docs) folder to find more interesting things that can be useful.

### Code-Based Configuration
The simplest configuration require [Telegram bot token](https://core.telegram.org/bots#generating-an-authentication-token) and [channel ID](https://community.jamaicans.dev/t/get-the-telegram-channel-id/427). Check the enclosed links to understand how to receive them.

```cs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Telegram(
        token: "0000000000:0000_000000000000000000000000000000",
        chatId: "-0000000000000")
    .CreateLogger();
```

For more complex configuration examples please check the related wiki page or browse the examples folder.

Sink also can be configured by JSON configuration file.
### JSON-Based Configuration
#### *Microsoft.Extensions.Configuration* package required
Keys and values are case-insensitive.

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
      }
    }
  ],
}
```

## Roadmap
Project's roadmap described at [Roadmap](./docs/roadmap.md).

## Contributing
Feel free to add any improvements you want via pull requests. All pull requests must be linked to an issue.

