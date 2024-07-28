using Example.Core;
using X.Extensions.Logging.Telegram;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();