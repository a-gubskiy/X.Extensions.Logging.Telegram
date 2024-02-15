using System.Collections.Immutable;
using X.Serilog.Sinks.Telegram.Batch.Rules;
using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Extensions;

public static class DependencyInjectionExtensions
{
    public static LoggerSinkConfiguration TelegramCore(
        this LoggerSinkConfiguration sinkConfig,
        string token,
        string chatId,
        LogEventLevel logLevel
    )
    {
        ArgumentNullException.ThrowIfNull(sinkConfig);
        ArgumentNullException.ThrowIfNull(token);
        ArgumentNullException.ThrowIfNull(chatId);

        TelegramCoreInternal(sinkConfig, token, chatId, logLevel);

        return sinkConfig;
    }

    private static void TelegramCoreInternal(LoggerSinkConfiguration sinkConfig, string token, string chatId,
        LogEventLevel logLevel)
    {
        sinkConfig.Telegram(config =>
            {
                config.Token = token;
                config.ChatId = chatId;

                config.Mode = TelegramSinkDefaults.DefaultFormatterMode;

                config.BatchPostingLimit = TelegramSinkDefaults.BatchPostingLimit;
                config.BatchEmittingRulesConfiguration = new BatchEmittingRulesConfiguration
                {
                    RuleCheckPeriod = TelegramSinkDefaults.RulesCheckPeriod,
                    BatchProcessingRules = new ImmutableArray<IRule>
                    {
                        // send logs to the Telegram once per 250 seconds
                        new OncePerTimeRule(TelegramSinkDefaults.RulesCheckPeriod * 50)
                    }
                };
                config.FormatterConfiguration = TelegramSinkDefaults.DefaultFormatterConfiguration;
            },
            messageFormatter: null!,
            restrictedToMinimumLevel: logLevel);
    }
}