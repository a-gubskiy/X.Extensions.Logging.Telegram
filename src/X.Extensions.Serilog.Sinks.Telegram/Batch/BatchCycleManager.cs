using System.Collections.Immutable;
using System.Threading;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Rules;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch;

internal class BatchCycleManager : IDisposable
{
    private readonly IImmutableList<IRule> _batchPositingRules;
    private readonly IImmutableList<IExecutionHook> _executionHooks;
    private readonly PeriodicTimer _timer;

    public BatchCycleManager(BatchEmittingRulesConfiguration configuration)
    {
        _batchPositingRules = configuration.BatchProcessingRules;
        _executionHooks = configuration.BatchProcessingExecutionHooks;

        _timer = new PeriodicTimer(configuration.RuleCheckPeriod);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    internal async Task WhenNextAvailableAsync(CancellationToken cancellationToken)
    {
        while (await _timer.WaitForNextTickAsync(cancellationToken))
        {
            var isAtLeastOneRulePassed =
                (await Task.WhenAll(_batchPositingRules.Select(rule => rule.IsPassedAsync(cancellationToken))))
                .Any(ruleResponse => ruleResponse);

            if (isAtLeastOneRulePassed)
            {
                break;
            }
        }
    }

    internal async Task OnBatchProcessedAsync(CancellationToken cancellationToken)
    {
        await Task.WhenAll(_executionHooks.Select(hook => hook.OnAfterExecuteAsync(cancellationToken)));
    }
}