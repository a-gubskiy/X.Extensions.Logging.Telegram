using System.Threading;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;

/// <summary>
/// Defines method to execute logic after running a piece of code that implements <see cref="IPostExecutionHook"/>.
/// </summary>
public interface IPostExecutionHook
{
    /// <summary>
    /// Executes a piece of logic after a batch of logs is sent.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask AfterExecuteAsync(CancellationToken cancellationToken);
}