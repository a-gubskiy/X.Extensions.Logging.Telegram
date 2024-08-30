using System.Threading;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;

/// <summary>
/// Batch emitting rule
/// </summary>
/// <remarks>
/// Rules are checked once per <see cref="BatchEmittingRulesConfiguration.RuleCheckPeriod"/>
/// </remarks>
public interface IRule
{
    /// <summary>
    /// Verifies if a batch can be emitted
    /// </summary>
    bool IsPassed();
}

/// <summary>
/// Async batch emitting rule
/// </summary>
/// <remarks>
/// Rules are checked once per <see cref="BatchEmittingRulesConfiguration.RuleCheckPeriod"/>
/// </remarks>
public interface IRuleAsync
{
    /// <summary>
    /// Verifies if a batch can be emitted
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask<bool> IsPassedAsync(CancellationToken cancellationToken);
}