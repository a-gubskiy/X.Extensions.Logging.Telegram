using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Formatters;

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
            LoggingMode mode = LoggingMode.Notifications,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = TelegramSinkDefaults.BatchPostingLimit,
            TimeSpan? period = null)
        {
            var config = new TelegramSinkConfiguration()
            {
                Token = token,
                ChatId = chatId,
                BatchPostingLimit = batchPostingLimit,
                Mode = mode,
                FormatterConfiguration = new FormatterConfiguration()
                {
                    UseEmoji = useEmoji,
                    ReadableApplicationName = readableApplicationName,
                }
            };

            if (period.HasValue)
            {
                config.BatchPeriod = period.Value;
            }

            config.Validate();

            return loggerConfiguration.Sink(new TelegramSink(messageFormatter, config),
                restrictedToMinimumLevel);
        }

        /// <summary>
        ///     Adds a sink that writes log events as telegram messages to a specified channel.
        ///     Fluent configuration.
        /// </summary>
        public static LoggerConfiguration Telegram(
            this LoggerSinkConfiguration loggerConfiguration,
            Action<TelegramSinkConfiguration> configureAction,
            IMessageFormatter messageFormatter,
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
                new TelegramSink(messageFormatter, config),
                restrictedToMinimumLevel);
        }
    }
}