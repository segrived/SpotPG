using System;

namespace SpotPG.Logger.Abstractions
{
    public class LogEventArgs : EventArgs
    {
        public LogItem LogItem { get; }

        public LogEventArgs(LogItem logItem)
        {
            this.LogItem = logItem with { };
        }
    }
}