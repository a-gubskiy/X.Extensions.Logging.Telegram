using Example.Core;
using X.Extensions.Logging.Telegram;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddTelegram(options =>
{
    options.ChatId = ExampleAppSettings.ChatId;
    options.AccessToken = ExampleAppSettings.Token;
    options.UseEmoji = true;
    options.LogLevel = new Dictionary<string, LogLevel> { { "Default", LogLevel.Information } };
    options.Source = "Example.WebApp";
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();