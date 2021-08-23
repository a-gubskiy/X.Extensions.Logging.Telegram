namespace X.Serilog.Sinks.Telegram.Sinks.Telegram
{
    public class FormatterConfiguration
    {
        public bool UseEmoji { get; set; }
        public string ReadableApplicationName { get; set; }
        public IMessageFormatter Formatter { get; set; }
    }
}