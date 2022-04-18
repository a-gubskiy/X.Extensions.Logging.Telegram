using Serilog;
using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, lc) => lc
    .WriteTo.Telegram(config =>
    {
        config.Token = "0000000000:xxxxxxxxxxxxxxxxx-xxxxxxx-xxxxxxx";
        config.ChatId = "-0000000000000";
        config.FormatterConfiguration = new FormatterConfiguration()
        {
            UseEmoji = true,
            ReadableApplicationName = "WebApp Example"
        };
        config.Mode = LoggingMode.Logs;
        config.BatchPostingLimit = 10;
    }, null!));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();