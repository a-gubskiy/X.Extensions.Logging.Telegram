using System;

using Serilog;
using Serilog.Configuration;
using Serilog.Events;

using X.Serilog.Sinks.Telegram.Sinks.Telegram;

namespace X.Serilog.Sinks.Telegram
{
    public static class LoggerConfigurationTelegramExtensions
    {
        /// <summary>
        ///     Adds a sink that writes log events as telegram messages to a specified channel.
        ///     For appsettings configuration.
        /// </summary>
        public static LoggerConfiguration Telegram(
            this LoggerSinkConfiguration loggerConfiguration,
            string token,
            string chatId,
            string readableApplicationName = "",
            bool useEmoji = false,
            IMessageFormatter messageFormatter = null,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = TelegramSinkDefaults.BatchPostingLimit,
            TimeSpan? period = null)
        {
            var config = new TelegramSinkConfiguration()
            {
                Token = token,
                ChatId = chatId,
                BatchPostingLimit = batchPostingLimit,
                FormatterConfiguration = new FormatterConfiguration()
                {
                    UseEmoji = useEmoji,
                    ReadableApplicationName = readableApplicationName,
                    Formatter = messageFormatter,
                }
            };

            if (period.HasValue)
            {
                config.BatchPeriod = period.Value;
            }

            config.Validate();

            return loggerConfiguration.Sink(new TelegramSink(config),
                restrictedToMinimumLevel);
        }

        /// <summary>
        ///     Adds a sink that writes log events as telegram messages to a specified channel.
        ///     Fluent configuration.
        /// </summary>
        public static LoggerConfiguration Telegram(
            this LoggerSinkConfiguration loggerConfiguration,
            Action<TelegramSinkConfiguration> configureAction,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            if (loggerConfiguration == null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            if (configureAction == null)
            {
                throw new ArgumentNullException(nameof(configureAction));
            }

            var config = new TelegramSinkConfiguration();

            configureAction(config);

            config.Validate();

            return loggerConfiguration.Sink(
                new TelegramSink(config),
                restrictedToMinimumLevel);
        }
    }
}