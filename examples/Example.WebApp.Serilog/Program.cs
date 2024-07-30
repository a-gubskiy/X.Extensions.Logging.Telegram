using System.Collections.Immutable;
using Example.Core;
using Example.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Rules;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<LogsService>();
builder.Host.UseSerilog((_, lc) => lc
    .WriteTo.Console()
    .WriteTo.Telegram(config =>
    {
        config.Token = ExampleAppSettings.Token;
        config.ChatId = ExampleAppSettings.ChatId;
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
            ReadableApplicationName = "Example.WebApp.Serilog",
            IncludeException = false,
            IncludeProperties = false,
            TimeZone = TimeZoneInfo.Utc
        };
        config.BatchPostingLimit = 10;
        config.Mode = LoggingMode.Logs;
    }, null!, LogEventLevel.Information));

var app = builder.Build();

app.MapGet("logs",
    (
        [FromServices] LogsService logsService) =>
    {
        logsService.WriteLogs();
    });

app.Run();