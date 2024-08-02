using System.Collections.Immutable;
using System.Threading;
using System.Threading.Channels;
using Serilog.Core;
using X.Extensions.Logging.Telegram.Base;
using X.Extensions.Logging.Telegram.Base.Formatters;
using X.Extensions.Serilog.Sinks.Telegram.Batch;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;
using X.Extensions.Serilog.Sinks.Telegram.Extensions;
using X.Extensions.Serilog.Sinks.Telegram.Filters;

namespace X.Extensions.Serilog.Sinks.Telegram;

public class TelegramSink : ILogEventSink, IAsyncDisposable
{
    private readonly BatchCycleManager _batchCycleManager;

    private readonly ChannelWriter<LogEvent> _channelWriter;
    private readonly ILogsQueueAccessor _logsQueueAccessor;

    private readonly ILogWriter _logWriter;

    private readonly TelegramSinkConfiguration _sinkConfiguration;

    private readonly LogsQueueProcessor _logsQueueProcessor;
    private readonly LogFilterManager? _logFilterManager;

    public TelegramSink(
        ChannelWriter<LogEvent> channelWriter,
        ILogsQueueAccessor logsQueueAccessor,
        TelegramSinkConfiguration sinkConfiguration,
        IMessageFormatter? messageFormatter)
    {
        _channelWriter = channelWriter;
        _logsQueueAccessor = logsQueueAccessor;
        _sinkConfiguration = sinkConfiguration;

        _logWriter = new TelegramLogWriter(_sinkConfiguration.Token, _sinkConfiguration.ChatId);
        _batchCycleManager = new BatchCycleManager(sinkConfiguration.BatchEmittingRulesConfiguration);

        if (sinkConfiguration.LogFiltersConfiguration is { ApplyLogFilters: true })
        {
            _logFilterManager = new LogFilterManager(sinkConfiguration.LogFiltersConfiguration);
        }

        var logFormatter = messageFormatter ??
                           TelegramSinkDefaults.GetDefaultMessageFormatter(_sinkConfiguration.Mode);
        _logsQueueProcessor = new LogsQueueProcessor(logsQueueAccessor, logFormatter, sinkConfiguration);

        ExecuteLogsProcessingLoop(CancellationToken.None);
    }

    public void Emit(LogEvent logEvent)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        var isFilterPassed = _logFilterManager?.ApplyFilter(logEvent);
        if (isFilterPassed is false)
        {
            return;
        }

        _channelWriter.TryWrite(logEvent);
    }

    private void ExecuteLogsProcessingLoop(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await _batchCycleManager.WhenNextAvailableAsync(cancellationToken);
                await EmitBatchAsync();
                await _batchCycleManager.AfterBatchProcessedAsync(cancellationToken);
            }
        }, cancellationToken);
    }

    private async Task EmitBatchAsync()
    {
        var batchSize = _sinkConfiguration.BatchPostingLimit;
        var messages = await _logsQueueProcessor.GetMessagesFromQueueAsync(batchSize);
        await SendMessagesAsync(messages);
    }

    private async Task EmitBatchInternalAsync(int batchSize)
    {
        var logs = await _logsQueueProcessor.GetMessagesFromQueueAsync(batchSize);
        await SendMessagesAsync(logs);
    }

    private async Task SendMessagesAsync(IImmutableList<string> messages)
    {
        if (!messages.Any())
        {
            return;
        }

        foreach (var message in messages)
        {
            await _logWriter.Write(message, CancellationToken.None);
        }
    }

    private async Task FlushAsync()
    {
        var batchSize = _sinkConfiguration.BatchPostingLimit;
        var requiredBatches = Math.Floor(_logsQueueAccessor.GetSize() / (double)batchSize);

        while (requiredBatches > 0)
        {
            await EmitBatchInternalAsync(batchSize);
            requiredBatches--;
        }
    }
    
    public static LogEntry ConvertToLogEntry(LogEvent logEvent)
    {
        ArgumentNullException.ThrowIfNull(logEvent);

        return new LogEntry
        {
            Message = logEvent.RenderMessage(),
            Level = logEvent.Level.ToTelegramLogLevel(),
            UtcTimeStamp = logEvent.Timestamp.ToUniversalTime().UtcDateTime,
            Exception = logEvent.Exception?.ToString(),
            Properties = logEvent.Properties.ToDictionary(x => x.Key, x => x.Value.ToString())
        };
    }

    public async ValueTask DisposeAsync()
    {
        _batchCycleManager.Dispose();
        _channelWriter.Complete();

        if (_logsQueueAccessor.GetSize() >= 0)
        {
            await FlushAsync();
        }
    }
}