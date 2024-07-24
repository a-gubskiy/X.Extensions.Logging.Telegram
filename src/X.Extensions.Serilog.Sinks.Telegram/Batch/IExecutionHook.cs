using System.Threading;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch;

/// <summary>
/// Executes some logic after running a piece of code the implements IExecutionHook 
/// </summary>
public interface IExecutionHook
{
    /// <summary>
    /// Callback to run
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task OnAfterExecuteAsync(CancellationToken cancellationToken);
}