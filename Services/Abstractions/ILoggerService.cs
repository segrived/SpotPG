using System;
using SpotPG.Services.Logger;

namespace SpotPG.Services.Abstractions
{
    public interface ILoggerService
    {
        ILogger CreateLogger();

        event EventHandler<LogEventArgs> OnNewMessage;
    }

    public class LogEventArgs : EventArgs
    {
        public string Text { get; set; }

        public LogType Type { get; set; }

        public LogEventArgs(string text, LogType type)
        {
            this.Text = text;
            this.Type = type;
        }
    }
}