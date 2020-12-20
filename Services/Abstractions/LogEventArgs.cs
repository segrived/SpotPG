using System;
using SpotPG.Services.Logger;

namespace SpotPG.Services.Abstractions
{
    public class LogEventArgs : EventArgs
    {
        public string Text { get; }

        public LogType Type { get; }

        public LogEventArgs(string text, LogType type)
        {
            this.Text = text;
            this.Type = type;
        }
    }
}