using System.Threading.Channels;
using X.Extensions.Logging.Telegram.Base.Formatters;
using X.Extensions.Serilog.Sinks.Telegram.Batch;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;

namespace X.Extensions.Serilog.Sinks.Telegram.Extensions;

public static class LoggerConfigurationTelegramExtensions
{
    /// <summary>
    ///     Adds a sink that writes log events as telegram messages to a specified channel.
    ///     Fluent configuration.
    /// </summary>
    public static LoggerConfiguration Telegram(
        this LoggerSinkConfiguration loggerConfiguration,
        Action<TelegramSinkConfiguration> configureAction,
        IMessageFormatter telegramMessageFormatter,
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

        var channel = Channel.CreateUnbounded<LogEvent>();
        var logsAccessor = new LogsQueueAccessContext(channel.Reader);

        var config = new TelegramSinkConfiguration(logsAccessor);
        configureAction(config);
        config.Validate();

        return loggerConfiguration.Sink(
            new TelegramSink(
                channel.Writer,
                logsAccessor,
                config,
                telegramMessageFormatter),
            restrictedToMinimumLevel);
    }
}