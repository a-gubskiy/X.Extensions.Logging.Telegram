using System;
using System.Collections.Generic;

namespace X.Extensions.Logging.Telegram.Base;

public class LogEntry
{
    public LogEntry()
    {
    }

    public TelegramLogLevel Level { get; set; }

    public DateTime UtcTimeStamp { get; set; }

    public string? Message { get; set; }

    public Dictionary<string, string>? Properties { get; set; }

    public string? Exception { get; set; }
}