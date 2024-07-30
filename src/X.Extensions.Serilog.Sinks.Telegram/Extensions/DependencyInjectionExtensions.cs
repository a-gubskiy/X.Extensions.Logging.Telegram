using System.Collections.Immutable;
using X.Extensions.Logging.Telegram.Base.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Rules;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;

namespace X.Extensions.Serilog.Sinks.Telegram.Extensions;

public static class DependencyInjectionExtensions
{
    public static LoggerConfiguration TelegramCore(
        this LoggerSinkConfiguration sinkConfig,
        string token,
        string chatId,
        LogEventLevel logLevel
    )
    {
        ArgumentNullException.ThrowIfNull(sinkConfig);
        ArgumentNullException.ThrowIfNull(token);
        ArgumentNullException.ThrowIfNull(chatId);

        return TelegramCoreInternal(sinkConfig, token, chatId, logLevel);
    }

    private static LoggerConfiguration TelegramCoreInternal(
        LoggerSinkConfiguration sinkConfig,
        string token,
        string chatId,
        LogEventLevel logLevel)
    {
        return sinkConfig.Telegram(config =>
            {
                config.Token = token;
                config.ChatId = chatId;

                config.Mode = TelegramSinkDefaults.DefaultFormatterMode;

                config.BatchPostingLimit = TelegramSinkDefaults.BatchPostingLimit;
                config.BatchEmittingRulesConfiguration = new BatchEmittingRulesConfiguration
                {
                    RuleCheckPeriod = TelegramSinkDefaults.RulesCheckPeriod,
                    BatchProcessingRules = new List<IRule>
                    {
                        new BatchSizeRule(config.LogsAccessor, batchSize: config.BatchPostingLimit),
                        // send logs to the Telegram once per 250 seconds
                        new OncePerTimeRule(TelegramSinkDefaults.RulesCheckPeriod * 50)
                    }.ToImmutableList()
                };
                config.FormatterConfiguration = FormatterConfiguration.Default;
                config.LogFiltersConfiguration = new LogsFiltersConfiguration
                {
                    ApplyLogFilters = false
                };
            },
            logFormatter: null!,
            restrictedToMinimumLevel: logLevel);
    }
}