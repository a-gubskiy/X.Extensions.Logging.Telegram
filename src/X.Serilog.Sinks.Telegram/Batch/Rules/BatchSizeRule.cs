using System.Threading;
using X.Serilog.Sinks.Telegram.Configuration;

namespace X.Serilog.Sinks.Telegram.Batch.Rules;

/// <summary>
/// Emit logs batch when in the logs queue is not less than <see cref="TelegramSinkConfiguration.BatchPostingLimit"/> entries.
/// </summary>
public class BatchSizeRule : IRule
{
    private readonly ILogsQueueAccessor _accessContext;
    private readonly int _batchSize;

    public BatchSizeRule(ILogsQueueAccessor accessContext, int batchSize)
    {
        _batchSize = batchSize;
        _accessContext = accessContext;
    }

    public Task<bool> IsPassedAsync(CancellationToken cancellationToken)
    {
        var isPass = _accessContext.GetSize() >= _batchSize;
        return Task.FromResult(isPass);
    }
}