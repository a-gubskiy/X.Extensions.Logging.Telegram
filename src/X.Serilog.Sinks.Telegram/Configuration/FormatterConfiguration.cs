namespace X.Serilog.Sinks.Telegram.Configuration;

public class FormatterConfiguration
{
    public bool UseEmoji { get; set; }
    public string ReadableApplicationName { get; set; }
    public bool IncludeException { get; set; }
    public bool IncludeProperties { get; set; }
}