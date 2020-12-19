using System;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services.Logger
{
    public class LoggerService : ILoggerService
    {
        public event EventHandler<LogEventArgs> OnNewMessage;

        public ILogger CreateLogger()
        {
            var logger = new Logger();
            logger.OnNewMessage += this.Logger_OnNewMessage;

            return logger;
        }

        private void Logger_OnNewMessage(object sender, LogEventArgs e)
            => this.OnNewMessage?.Invoke(this, new LogEventArgs(e.Text, e.Type));

        private class Logger : ILogger
        {
            internal event EventHandler<LogEventArgs> OnNewMessage;

            public void Log(string text, LogType type)
            {
                this.OnNewMessage?.Invoke(this, new LogEventArgs(text, type));
            }
        }
    }
}