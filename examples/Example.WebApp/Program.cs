using Example.Core;
using Example.Core.Services;
using Microsoft.AspNetCore.Mvc;
using X.Extensions.Logging.Telegram;
using X.Extensions.Logging.Telegram.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<LogsService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddTelegram(options =>
{
    options.ChatId = ExampleAppSettings.ChatId;
    options.AccessToken = ExampleAppSettings.Token;
    options.FormatterConfiguration.UseEmoji = true;
    options.FormatterConfiguration.ReadableApplicationName = "Example.WebApp";
    options.LogLevel = new Dictionary<string, LogLevel> { { "Default", LogLevel.Information } };     
});

var app = builder.Build();

app.MapGet("logs",
    (
        [FromServices] LogsService logsService) =>
    {
        logsService.WriteLogs();
    });

app.Run();