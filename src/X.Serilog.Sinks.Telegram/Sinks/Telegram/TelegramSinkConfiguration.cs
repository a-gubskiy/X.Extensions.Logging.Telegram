using System;

namespace X.Serilog.Sinks.Telegram.Sinks.Telegram
{
    public class TelegramSinkConfiguration
    {
        private string _token;
        private string _chatId;
        private int _batchPostingLimit;
        private TimeSpan _batchPeriod = TelegramSinkDefaults.BatchPostingPeriod;

        public string Token
        {
            get => _token;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Invalid token! Token must be not null, empty or whitespace!");
                }

                _token = value;
            }
        }

        public string ChatId
        {
            get => _chatId;
            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || !long.TryParse(value, out _))
                {
                    throw new ArgumentException("Invalid chat id! It must be not null, empty or whitespace " +
                                                "and it's should be a number!");
                }

                _chatId = value;
            }
        }

        public int BatchPostingLimit
        {
            get => _batchPostingLimit;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Invalid batch posting limit! It must be greater than 0!");
                }

                _batchPostingLimit = value;
            }
        }

        public TimeSpan BatchPeriod
        {
            get => _batchPeriod;
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Invalid batch period! It must be greater than TimeSpan.Zero!");
                }

                _batchPeriod = value;
            }
        }

        public FormatterConfiguration FormatterConfiguration { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Token) || string.IsNullOrWhiteSpace(Token))
            {
                throw new ArgumentException("Invalid token! Token must be not null, empty or whitespace!");
            }

            if (string.IsNullOrEmpty(ChatId) || string.IsNullOrWhiteSpace(ChatId) || !long.TryParse(ChatId, out _))
            {
                throw new ArgumentException("Invalid chat id! It must be not null, empty or whitespace " +
                                            "and it's should be a number!");
            }

            if (BatchPostingLimit <= 0)
            {
                throw new ArgumentException("Invalid batch posting limit! It must be greater than 0!");
            }

            if (FormatterConfiguration is null)
            {
                throw new ArgumentNullException(nameof(FormatterConfiguration));
            }
        }
    }
}