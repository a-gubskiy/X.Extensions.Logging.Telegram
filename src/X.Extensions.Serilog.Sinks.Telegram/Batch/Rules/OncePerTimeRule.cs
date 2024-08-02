using System.Threading;
using X.Extensions.Serilog.Sinks.Telegram.Batch.Contracts;
using X.Extensions.Serilog.Sinks.Telegram.Configuration;

namespace X.Extensions.Serilog.Sinks.Telegram.Batch.Rules;

/// <summary>
/// Emit logs batch when configured delay passed.
/// </summary>
public class OncePerTimeRule : IRule, IPostExecutionHook
{
    private readonly TimeSpan _delay;
    private DateTime _nextExecution;

    /// <summary>
    /// Logs' the batch posting limit.
    /// Once the delay is reached, the logs will be posted even if
    /// there are fewer than <see cref="TelegramSinkConfiguration.BatchPostingLimit"/> logs in the queue.
    /// </summary>
    /// <param name="delay">The time limit for batch posting.</param>
    /// <exception cref="ArgumentException">
    /// Thrown if the delay is less than or equal to <see cref="TimeSpan.Zero"/>.
    /// </exception>
    /// <remarks>
    /// Although it is technically possible to set very short delays,
    /// doing so can significantly impact your application's performance.
    /// Therefore, it is not recommended to set the delay to less than the default value.
    /// </remarks>
    public OncePerTimeRule(TimeSpan delay)
    {
        if (delay <= TimeSpan.Zero)
        {
            throw new ArgumentException("Invalid batch period! It must be greater than TimeSpan.Zero!");
        }

        _delay = delay;
        _nextExecution = DateTime.Now + delay;
    }

    public ValueTask AfterExecuteAsync(CancellationToken cancellationToken)
    {
        _nextExecution = DateTime.Now + _delay;
        return ValueTask.CompletedTask;
    }

    public bool IsPassed()
    {
        var now = DateTime.Now;

        var isPassed = now >= _nextExecution;
        if (isPassed)
        {
            _nextExecution = now + _delay;
        }

        return isPassed;
    }
}