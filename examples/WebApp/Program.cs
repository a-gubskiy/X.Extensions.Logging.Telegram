using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Events;
using X.Serilog.Sinks.Telegram.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, lc) => lc
    .WriteTo.Telegram(config =>
    {
        config.Token = "0000000000:0000000000000000000-0000000-0000000";
        config.ChatId = "-000000000000";
        config.FormatterConfiguration = new FormatterConfiguration
        {
            UseEmoji = true,
            ReadableApplicationName = "WebApp Example",
            IncludeException = true,
            IncludeProperties = true
        };
        config.Mode = LoggingMode.Logs;
        config.BatchPostingLimit = 10;
        config.BatchPeriod = TimeSpan.FromSeconds(5);
        config.RuleCheckPeriod = TimeSpan.FromMilliseconds(100);
    }, null!, LogEventLevel.Information));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapPost("logs/generate",
    (
        GenerateLogsRequest request,
        [FromServices] ILogger<Program> logger) =>
    {
        const string testMessage = "Test message";

        var amount = request.Amount;
        while (amount > 0)
        {
            var logLevel = request.LogLevel;
            logger.Log(logLevel, testMessage);

            amount--;
        }
    });


app.Run();


internal record GenerateLogsRequest(
    [JsonConverter(typeof(StringEnumConverter))]
    LogLevel LogLevel,
    int Amount);