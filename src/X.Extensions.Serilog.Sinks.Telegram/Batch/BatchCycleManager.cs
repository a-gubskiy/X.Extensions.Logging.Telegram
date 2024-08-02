using System.Collections.Immutable;
using System.Threading;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Rules;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch;

internal class BatchCycleManager : IDisposable
{
    private readonly IImmutableList<IRule> _batchPositingRules;
    private readonly IImmutableList<IRuleAsync> _asycnBatchPositingRules;
    private readonly IImmutableList<IPostExecutionHook> _postExecutionHooks;
    private readonly List<bool> _ruleChecksBuffer;
    private readonly PeriodicTimer _timer;

    public BatchCycleManager(BatchEmittingRulesConfiguration configuration)
    {
        _batchPositingRules = configuration.BatchProcessingRules;
        _asycnBatchPositingRules = configuration.AsyncBatchProcessingRules;
        _postExecutionHooks = configuration.BatchPostExecutionHooks;

        _ruleChecksBuffer = new List<bool>(_batchPositingRules.Count + _asycnBatchPositingRules.Count);
        _timer = new PeriodicTimer(configuration.RuleCheckPeriod);
    }

    internal async Task WhenNextAvailableAsync(CancellationToken cancellationToken)
    {
        while (await _timer.WaitForNextTickAsync(cancellationToken))
        {
            CheckSyncRules();
            await CheckAsyncRules(cancellationToken);

            var isAtLeastOneRulePassed = _ruleChecksBuffer.Any(r => r);
            _ruleChecksBuffer.Clear();

            if (isAtLeastOneRulePassed)
            {
                break;
            }
        }
    }

    private void CheckSyncRules()
    {
        if (_batchPositingRules.Any())
        {
            _ruleChecksBuffer.AddRange(
                _batchPositingRules.Select(rule => rule.IsPassed())
            );
        }
    }

    private async Task CheckAsyncRules(CancellationToken cancellationToken)
    {
        if (_asycnBatchPositingRules.Any())
        {
            _ruleChecksBuffer.AddRange(
                await Task.WhenAll(
                    _asycnBatchPositingRules.Select(rule => rule.IsPassedAsync(cancellationToken).AsTask())
                )
            );
        }
    }

    internal async Task AfterBatchProcessedAsync(CancellationToken cancellationToken)
    {
        var executionHooksTasks = _postExecutionHooks
            .Select(hook => hook.AfterExecuteAsync(cancellationToken).AsTask());
        await Task.WhenAll(executionHooksTasks);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}