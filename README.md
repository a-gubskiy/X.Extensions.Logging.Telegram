# X.Serilog.Sinks.Telegram

[![Build status](https://ci.appveyor.com/api/projects/status/n4uj9qfuywrkdrhb/branch/main?svg=true)](https://ci.appveyor.com/project/Bardin08/x-serilog-sinks-telegram/branch/main)

[![NuGet](https://img.shields.io/nuget/v/X.Serilog.Sinks.Telegram)](https://www.nuget.org/packages/X.Serilog.Sinks.Telegram)
[![NuGet Downloads](https://img.shields.io/nuget/dt/X.Serilog.Sinks.Telegram)](https://www.nuget.org/packages/X.Serilog.Sinks.Telegram)

## Table of Contents

- [X.Serilog.Sinks.Telegram](#xserilogsinkstelegram)
  - [Introduction](#introduction)
  - [Features](#features)
  - [Getting Started](#getting-started)
  - [Examples](#examples)
  - [Contributing](#contributing)
  - [License](#license)

## Introduction

X.Serilog.Sinks.Telegram is an open-source Serilog sink that allows you to send log events to Telegram. It's a convenient way to integrate Telegram as a logging output, enabling you to receive important log information directly in your chat.

## Features

- **Real-time Logging**: The sink offers the ability to send log events to a Telegram channel in real-time, ensuring that you can stay up-to-date with your application's behavior and any issues as they arise.

- **Customizable Formatting**: You can configure the format of log messages sent to the Telegram channel, allowing you to tailor them to your preferences and specific requirements.

- **Filtering**: The sink supports filtering log events before they are dispatched to the Telegram channel, ensuring that only pertinent information is shared.

- **Asynchronous Sending**: Log events are sent asynchronously to the Telegram channel, minimizing potential impact on your application's performance.

- **Easy Configuration**: Configuring the sink to work with your Telegram channel is straightforward, and you can find comprehensive information in the [Configuration Wiki](https://github.com/Bardin08/X.Serilog.Sinks.Telegram/wiki/Configuration).

## Getting Started

To begin using the X.Serilog.Sinks.Telegram sink, follow these steps:

1. **Install the Package**: You can install the sink package from NuGet using the following command:
```shell
dotnet add package X.Serilog.Sinks.Telegram
```

2. **Configure the Sink**: Set up the Telegram sink with the appropriate settings in your application's configuration. Here's an example configuration in C#:

```c#
Log.Logger = new LoggerConfiguration()
    .WriteTo.TelegramCore(
        token: botToken,
        chatId: loggingChatId,
        logLevel: LogEventLevel.Verbose)
    .WriteTo.Console()
    .CreateLogger();
```

3. **Start Logging**: Once the sink is configured, you can log in using Serilog as usual. Log events will be sent to your Telegram channel.

For more detailed configuration options, please refer to the [Configuration Wiki](https://github.com/Bardin08/X.Serilog.Sinks.Telegram/wiki/Configuration).

## Examples

This repository includes several example projects that demonstrate how to use X.Serilog.Sinks.Telegram in various scenarios. These examples can be helpful if you're starting or looking to use a specific feature.

You can find the examples in the following location: [X.Serilog.Sinks.Telegram Examples](https://github.com/Bardin08/X.Serilog.Sinks.Telegram/tree/main/examples)

## Contributing
Feel free to add any improvements you want via pull requests. All pull requests must be linked to an issue.

## License
This project is licensed under the [MIT License](https://en.wikipedia.org/wiki/MIT_License)https://en.wikipedia.org/wiki/MIT_License.
