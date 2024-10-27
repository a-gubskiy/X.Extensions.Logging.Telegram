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

### Use a custom log writer
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