using SpotPG.Logger.Abstractions;

namespace SpotPG.Frontend.Services;

public class LoggerService : ILoggerService
{
    private const int MAX_LOG_ITEMS_COUNT = 5000;

    private readonly LinkedList<LogItem> lastLogItems = new();

    public event EventHandler<LogEventArgs> OnNewMessage;

    public IEnumerable<LogItem> LastLogItems => this.lastLogItems.AsEnumerable();

    public IServiceLogger CreateLogger()
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

    private class Logger : IServiceLogger
    {
        internal event EventHandler<LogEventArgs> OnNewMessage;

        public void Log(string text, LogType type)
        {
            var logItem = new LogItem(text, type, DateTimeOffset.UtcNow);
            this.OnNewMessage?.Invoke(this, new LogEventArgs(logItem));
        }
    }
}