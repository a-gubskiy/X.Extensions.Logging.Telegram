[![NuGet Version](http://img.shields.io/nuget/v/X.Extensions.Logging.Telegram.svg?style=flat)](https://www.nuget.org/packages/X.Extensions.Logging.Telegram/)

# X.Extensions.Logging.Telegram
Telegram logging provider

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

**Do not forgot** to add your bot as admin with _write messages_ permission to channel.

## Configure Telegram Logging provider
You can configure Telegram logging provider by code or by config file:

### Code 
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


## Get a digital subscription for project news
[Subscribe](https://twitter.com/intent/user?screen_name=andrew_gubskiy) to my Twitter to keep up-to-date with project news and receive announcements.

[![Twitter URL](https://img.shields.io/twitter/url/https/twitter.com/andrew_gubskiy.svg?style=social&label=Follow%20%40andrew_gubskiy)](https://twitter.com/andrew_gubskiy)