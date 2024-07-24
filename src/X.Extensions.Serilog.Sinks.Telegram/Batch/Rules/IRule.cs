using System.Threading;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch.Rules;

/// <summary>
/// Batch emitting rule
/// </summary>
public interface IRule
{
    /// <summary>
    /// Verifies if a batch can be emitted
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<bool> IsPassedAsync(CancellationToken cancellationToken);
}