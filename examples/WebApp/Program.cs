using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Events;
using System.Collections.Immutable;
using X.Serilog.Sinks.Telegram.Batch.Rules;
using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, lc) => lc
    .WriteTo.Telegram(config =>
    {
        config.Token = "0000000000:0000000000000000000-0000000-0000000";
        config.ChatId = "-000000000000";
        config.BatchEmittingRulesConfiguration = new BatchEmittingRulesConfiguration
        {
            RuleCheckPeriod = TimeSpan.FromSeconds(5),
            BatchProcessingRules = new IRule[]
            {
                new BatchSizeRule(config.LogsAccessor, batchSize: 10),
                new OncePerTimeRule(TimeSpan.FromSeconds(30))
            }.ToImmutableList()
        };
        config.LogFiltersConfiguration = new LogsFiltersConfiguration
        {
            ApplyLogFilters = false,
        };
        config.FormatterConfiguration = new FormatterConfiguration
        {
            UseEmoji = true,
            ReadableApplicationName = "WebApp Example",
            IncludeException = false,
            IncludeProperties = false,
            TimeZone = TimeZoneInfo.Utc
        };
        config.BatchPostingLimit = 10;
        config.Mode = LoggingMode.Logs;
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