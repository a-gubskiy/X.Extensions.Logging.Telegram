using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace X.Extensions.Logging.Telegram
{
    [PublicAPI]
    public static class TelegramLoggerExtensions
    {
        /// <summary>
        /// Adds a Telegram logger named 'Telegram' to the factory.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder)
        {
            return builder.AddTelegram(new TelegramLoggerOptions());
        }

        /// <summary>
        /// Adds a Telegram logger named 'Telegram' to the factory.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, Action<TelegramLoggerOptions> options)
        {
            var config = new TelegramLoggerOptions();
            options(config);

            return builder.AddTelegram(config);
        }
        
        /// <summary>
        /// Adds a Telegram logger named 'Telegram' to the factory.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, IConfiguration configuration)
        {
            var options = new TelegramLoggerOptions();

            configuration.GetSection("Logging:Telegram")?.Bind(options);

            return builder.AddTelegram(options);
        }

        /// <summary>
        /// Adds a Telegram logger named 'Telegram' to the factory.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddTelegram(this ILoggingBuilder builder, TelegramLoggerOptions options)
        {
            var telegramLoggerProcessor = new TelegramLoggerProcessor(options);
            
            builder.AddProvider(new TelegramLoggerProvider(options, telegramLoggerProcessor));
            
            return builder;
        }
    }
}