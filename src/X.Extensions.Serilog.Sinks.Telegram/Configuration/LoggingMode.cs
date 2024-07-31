namespace X.Extensions.Serilog.Sinks.Telegram.Configuration;

public enum LoggingMode
{
    /// <summary>
    ///     Log messages will be published to the specified Telegram channel.
    /// </summary>
    /// <example>
    ///
    ///     <b>[23.08.21 21:52:42 ERR️]</b> Logger
    ///     <br/><br/>
    ///     <b>Message:</b> An error occured while handling user request<br/>
    ///     <b>Exception:</b> System.Exception: Test exception!<br/>
    ///     at Application.Program.Main() in ~\Application\Main.cs:line 9<br/>
    ///     <b>Properties:</b><br/>
    ///     {
    ///         "SourceContext": {
    ///             "Value": "Application.Program.Main"
    ///         },
    ///         "Application": {
    ///             "Value": "Test Application"
    ///         }
    ///     }
    /// </example>
    Logs,
        
    /// <summary>
    ///     Messages will contain an info about all notifications which were received during a batch period or batch limit.
    /// </summary>
    /// <example>
    ///     <b>[23.08.21 21:52:42 — 23.08.21 22:52:42]</b>
    ///     <br/><br/>
    ///     <b>[21:52:42 ERR]</b> An error occured while handling user request<br/>
    ///     <b>[22:30:12 ERR]</b> An error occured while request to https://www.integration.service.com<br/>
    ///     <b>[22:52:42 DBG]</b> User successfully logged<br/>
    /// </example>
    AggregatedNotifications,
}