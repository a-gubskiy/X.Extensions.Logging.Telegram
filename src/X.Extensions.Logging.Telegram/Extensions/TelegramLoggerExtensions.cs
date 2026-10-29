using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using X.Extensions.Logging.Telegram.Base;
using X.Extensions.Logging.Telegram.Base.Formatters;

namespace X.Extensions.Logging.Telegram.Extensions;

[PublicAPI]
public static class TelegramLoggerExtensions
{
    private const string ConfigurationSection = "Logging:Telegram";

    /// <summary>
    /// Adds a Telegram logger with default options.
    /// </summary>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder) =>
        builder.AddTelegram(new TelegramLoggerOptions());

    /// <summary>
    /// Adds a Telegram logger configured by the <paramref name="configure"/> delegate.
    /// </summary>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, Action<TelegramLoggerOptions> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new TelegramLoggerOptions();
        configure(options);

        return builder.AddTelegram(options);
    }

    /// <summary>
    /// Adds a Telegram logger configured from the <c>Logging:Telegram</c> configuration section.
    /// </summary>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var options = new TelegramLoggerOptions();
        configuration.GetSection(ConfigurationSection).Bind(options);

        return builder.AddTelegram(options);
    }

    /// <summary>
    /// Adds a Telegram logger with the specified options.
    /// </summary>
    public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, TelegramLoggerOptions options) =>
        builder.AddTelegram(options, new TelegramLogWriter(options.AccessToken, options.ChatId));

    /// <summary>
    /// Adds a Telegram logger which uses a custom <see cref="ILogWriter"/>.
    /// </summary>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options,
        ILogWriter logWriter) =>
        builder.AddTelegram(options, new LogQueueProcessor(logWriter));

    /// <summary>
    /// Adds a Telegram logger which uses a custom <see cref="ILogQueueProcessor"/>.
    /// </summary>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options,
        ILogQueueProcessor logQueueProcessor) =>
        builder.AddTelegram(options, logQueueProcessor, _ => new DefaultLogFormatter());

    /// <summary>
    /// Adds a Telegram logger which uses a custom message formatter.
    /// </summary>
    /// <param name="builder">The logging builder.</param>
    /// <param name="options">Logger options.</param>
    /// <param name="createFormatter">Creates a formatter for the given logger category.</param>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options,
        Func<string, IMessageFormatter> createFormatter)
    {
        var logWriter = new TelegramLogWriter(options.AccessToken, options.ChatId);

        return builder.AddTelegram(options, new LogQueueProcessor(logWriter), createFormatter);
    }

    /// <summary>
    /// Adds a Telegram logger with a custom <see cref="ILogQueueProcessor"/> and message formatter.
    /// </summary>
    /// <param name="builder">The logging builder.</param>
    /// <param name="options">Logger options.</param>
    /// <param name="logQueueProcessor">Processor which delivers formatted messages.</param>
    /// <param name="createFormatter">Creates a formatter for the given logger category.</param>
    public static ILoggingBuilder AddTelegram(
        this ILoggingBuilder builder,
        TelegramLoggerOptions options,
        ILogQueueProcessor logQueueProcessor,
        Func<string, IMessageFormatter> createFormatter)
    {
        builder.AddConfiguration();

        foreach (var logLevelConfiguration in options.LogLevel)
        {
            var category = logLevelConfiguration.Key == "Default" ? "" : logLevelConfiguration.Key;

            builder.AddFilter<TelegramLoggerProvider>(category, logLevelConfiguration.Value);
        }

        // Registered via factory so the container owns the provider and disposes it,
        // which flushes the message queue on shutdown
        builder.Services.AddSingleton<ILoggerProvider>(_ =>
            new TelegramLoggerProvider(options, logQueueProcessor, createFormatter));

        return builder;
    }
}
