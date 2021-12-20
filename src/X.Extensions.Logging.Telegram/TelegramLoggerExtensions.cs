using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace X.Extensions.Logging.Telegram;

[PublicAPI]
public static class TelegramLoggerExtensions
{
    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder)
    {
        return AddTelegram(builder, new TelegramLoggerOptions());
    }

    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, Action<TelegramLoggerOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new TelegramLoggerOptions();
        configure(options);

        return AddTelegram(builder, options);
    }

    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var options = new TelegramLoggerOptions();

        configuration.GetSection("Logging:Telegram")?.Bind(options);

        return AddTelegram(builder, options);
    }

    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, TelegramLoggerOptions options)
    {
        var telegramLoggerProcessor = new TelegramLoggerProcessor(options.AccessToken, options.ChatId);

        return AddTelegram(builder, options, new DefaultLogLevelChecker(), telegramLoggerProcessor);
    }
    
    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="telegramLoggerProcessor"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, TelegramLoggerOptions options, ITelegramLoggerProcessor telegramLoggerProcessor)
    {

        return AddTelegram(builder, options, new DefaultLogLevelChecker(), telegramLoggerProcessor);
    }

    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="logLevelChecker"></param>
    /// <param name="telegramLoggerProcessor"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options,
        ILogLevelChecker logLevelChecker,
        ITelegramLoggerProcessor telegramLoggerProcessor)
    {
        builder.AddConfiguration();
        
        foreach (var logLevelConfiguration in options.LogLevel)
        {
            var category = logLevelConfiguration.Key == "Default" ? "" : logLevelConfiguration.Key;
            var level = logLevelConfiguration.Value;
            
            builder.AddFilter<TelegramLoggerProvider>(category, level);
        }
            
        builder.AddProvider(new TelegramLoggerProvider(options, telegramLoggerProcessor, logLevelChecker));
            
        return builder;
    }
}