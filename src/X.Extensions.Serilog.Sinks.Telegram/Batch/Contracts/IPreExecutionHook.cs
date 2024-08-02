using System.Threading;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;

/// <summary>
/// Defines method to execute logic before running a piece of code that implements <see cref="IPreExecutionHook"/>.
/// </summary>
public interface IPreExecutionHook
{
    /// <summary>
    /// Executes a piece of logic before a batch of logs is sent.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask BeforeExecuteAsync(CancellationToken cancellationToken);
}