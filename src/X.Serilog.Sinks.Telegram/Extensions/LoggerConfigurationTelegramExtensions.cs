using System.Threading.Channels;
using X.Serilog.Sinks.Telegram.Batch;
using X.Serilog.Sinks.Telegram.Configuration;
using X.Serilog.Sinks.Telegram.Formatters;

namespace X.Serilog.Sinks.Telegram.Extensions;

public static class LoggerConfigurationTelegramExtensions
{
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
                messageFormatter),
            restrictedToMinimumLevel);
    }
}