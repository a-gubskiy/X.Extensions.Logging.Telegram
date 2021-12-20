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
        var logLevelChecker = new DefaultLogLevelChecker();
        var logWriter = new TelegramLogWriter(options.AccessToken, options.ChatId);
        var logQueueProcessor = new LogQueueProcessor(logWriter);

        return AddTelegram(builder, options, logLevelChecker, logQueueProcessor);
    }

    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="logWriter"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder, 
        TelegramLoggerOptions options,
        ILogWriter logWriter)
    {
        var logLevelChecker = new DefaultLogLevelChecker();
        var logQueueProcessor = new LogQueueProcessor(logWriter);

        return AddTelegram(builder, options, logLevelChecker, logQueueProcessor);
    }

    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="logQueueProcessor"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options, 
        ILogQueueProcessor logQueueProcessor)
    {
        var levelChecker = new DefaultLogLevelChecker();

        return AddTelegram(builder, options, levelChecker, logQueueProcessor);
    }

    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="logLevelChecker"></param>
    /// <param name="logQueueProcessor"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options,
        ILogLevelChecker logLevelChecker,
        ILogQueueProcessor logQueueProcessor)
    {
        ITelegramMessageFormatter CreateFormatter(string name) => new TelegramMessageFormatter(options, name);

        return AddTelegram(builder, options, logLevelChecker, logQueueProcessor, CreateFormatter);
    }
    
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options,
        Func<string, ITelegramMessageFormatter> createFormatter)
    {
        var logWriter = new TelegramLogWriter(options.AccessToken, options.ChatId);
        var logLevelChecker = new DefaultLogLevelChecker();
        var logQueueProcessor = new LogQueueProcessor(logWriter);

        return AddTelegram(builder, options, logLevelChecker, logQueueProcessor, createFormatter);
    }
    
    /// <summary>
    /// Adds a Telegram logger to the factory
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <param name="logLevelChecker"></param>
    /// <param name="logQueueProcessor"></param>
    /// <param name="createFormatter"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options,
        ILogLevelChecker logLevelChecker,
        ILogQueueProcessor logQueueProcessor, 
        Func<string, ITelegramMessageFormatter> createFormatter)
    {
        builder.AddConfiguration();
        
        foreach (var logLevelConfiguration in options.LogLevel)
        {
            var category = logLevelConfiguration.Key == "Default" ? "" : logLevelConfiguration.Key;
            var level = logLevelConfiguration.Value;
            
            builder.AddFilter<TelegramLoggerProvider>(category, level);
        }
            
        builder.AddProvider(new TelegramLoggerProvider(options, logQueueProcessor, logLevelChecker, createFormatter));
            
        return builder;
    }
}