using System;

namespace X.Extensions.Logging.Telegram.Base;

[Flags]
public enum TelegramLogLevel
{
    None        = 0,
    Trace       = 1 << 0, // 1
    Debug       = 1 << 1, // 2
    Information = 1 << 2, // 4
    Warning     = 1 << 3, // 8
    Error       = 1 << 4, // 16
    Critical    = 1 << 5, // 32
    All         = Trace | Debug | Information | Warning | Error | Critical // 63
}