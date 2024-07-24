# X.Extensions.Logging.Telegram & X.Serilog.Sinks.Telegram

Telegram logging provider for Serilog and standard .NET logging.

## What is Telegram?
What is Telegram? What do I do here?
Telegram is a messaging app with a focus on speed and security, it’s super-fast, simple and free. You can use Telegram on all your devices at the same time — your messages sync seamlessly across any number of your phones, tablets or computers. Telegram has over 500 million monthly active users and is one of the 10 most downloaded apps in the world.

## Why you need write logs to Telegram?
Because it very comfortable - you can receive important messages directly to your smartphone or laptop.

## Prepare Telegram bot
For sending log messages into telegram channel or chat you need create telegram bot before. [Here](https://core.telegram.org/bots#3-how-do-i-create-a-bot) you can find how to do it.
After you created bot add it to channel  with  admin role and allow bot to post messages.

## Prepare Telegram channel
In telegram there are two types of channels: public and private. For public channel you can use channel name as *ChatId* in configuration. 

For private channel you can use [@JsonDumpBot](https://t.me/jsondumpbot) to get private channel id. Just forward any message from private channelto this bot. Additional information you can find [here](https://botostore.com/c/jsondumpbot/).

**Do not forget** to add your bot as admin with _write messages_ permission to channel.


## X.Extensions.Logging.Telegram
[![NuGet](https://img.shields.io/nuget/v/X.Extensions.Logging.Telegram)](https://www.nuget.org/packages/X.Extensions.Logging.Telegram)
[![NuGet Downloads](https://img.shields.io/nuget/dt/X.Extensions.Logging.Telegram)](https://www.nuget.org/packages/X.Extensions.Logging.Telegram)

X.Extensions.Logging.Telegram is logging provider for standard .NET logging.

### Getting Started

You can configure Telegram logging provider by code or by config file:

```csharp
var options = new TelegramLoggerOptions(LogLevel.Information)
{
    AccessToken = "1234567890:AAAaaAAaa_AaAAaa-AAaAAAaAAaAaAaAAAA",
    ChatId = "-0000000000000",
    Source = "Human Readable Project Name"
};

...

builder
  .ClearProviders()
  .AddTelegram(options)
  .AddConsole();
                        
```

### appconfig.json

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "Telegram": {
      "LogLevel": {
        "Default": "Error",
        "WebApp.Controllers": "Warning"
      },
      "AccessToken": "1234567890:AAAaaAAaa_AaAAaa-AAaAAAaAAaAaAaAAAA",
      "ChatId": "1234567890",
      "Source": "Human Readable Project Name"
    }
  },
  "AllowedHosts": "*"
}
```

and pass IConfiguration object to extensions method

```
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureLogging((context, builder) =>
        {
            if (context.Configuration != null)
                builder
                    .AddTelegram(context.Configuration)
                    .AddConsole();
        })
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
````

### Use custom log writer
Now developers can use their own implementation for writing data to Telegram. Custom writer should implement _ILogWriter_ interface:

``` cs
var customLogWriter = new CustomLogWriter();
logBuilder.AddTelegram(options, customLogWriter);
```

### Use custom message formatter
For implement custom message formatting _ITelegramMessageFormatter_ can be used now.

``` cs
private ITelegramMessageFormatter CreateFormatter(string name)
{
    return new CustomTelegramMessageFormatter(name);
}

logBuilder.AddTelegram(options, CreateFormatter);
```

For using custom message formatter delegate Func<string, ITelegramMessageFormatter> should be passed to extensions method AddTelegram. Delegate should be used because formatter needs to know which category is used for rendering the message.




# X.Serilog.Sinks.Telegram

[![NuGet](https://img.shields.io/nuget/v/X.Serilog.Sinks.Telegram)](https://www.nuget.org/packages/X.Serilog.Sinks.Telegram)
[![NuGet Downloads](https://img.shields.io/nuget/dt/X.Serilog.Sinks.Telegram)](https://www.nuget.org/packages/X.Serilog.Sinks.Telegram)


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

# Examples

This repository includes several example projects that demonstrate how to use both libraries  in various scenarios. 
These examples can be helpful if you're starting or looking to use a specific feature.


# Contributing
Feel free to add any improvements you want via pull requests. All pull requests must be linked to an issue.

# License
This project is licensed under the MIT License.
