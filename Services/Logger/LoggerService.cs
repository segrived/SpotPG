using System;
using System.Collections.Generic;
using System.Linq;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        private const int MAX_LOG_ITEMS_COUNT = 5000;

        private readonly LinkedList<LogItem> lastLogItems = new();

        public event EventHandler<LogEventArgs> OnNewMessage;

        public IEnumerable<LogItem> LastLogItems => this.lastLogItems.AsEnumerable();

        public ILogger CreateLogger()
        {
            var logger = new Logger();
            logger.OnNewMessage += this.Logger_OnNewMessage;

            return logger;
        }

        private void Logger_OnNewMessage(object sender, LogEventArgs e)
        {
            this.OnNewMessage?.Invoke(this, new LogEventArgs(e.LogItem));

            this.lastLogItems.AddFirst(e.LogItem);

            if (this.lastLogItems.Count > MAX_LOG_ITEMS_COUNT)
                this.lastLogItems.RemoveLast();
        }

        private class Logger : ILogger
        {
            internal event EventHandler<LogEventArgs> OnNewMessage;

            public void Log(string text, LogType type)
            {
                var logItem = new LogItem(text, type, DateTimeOffset.UtcNow);
                this.OnNewMessage?.Invoke(this, new LogEventArgs(logItem));
            }
        }
    }

    public class LogEventArgs : EventArgs
    {
        public LogItem LogItem { get; }

        public LogEventArgs(LogItem logItem)
        {
            this.LogItem = logItem with { };
        }
    }

    public record LogItem(string Text, LogType Type, DateTimeOffset Date);
}