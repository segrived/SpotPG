using System;
using SpotPG.Services.Logger;

namespace SpotPG.Services.Abstractions
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