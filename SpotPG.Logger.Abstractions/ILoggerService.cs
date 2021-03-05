using System;
using System.Collections.Generic;

namespace SpotPG.Logger.Abstractions
{
    public interface ILoggerService
    {
        ILogger CreateLogger();

        event EventHandler<LogEventArgs> OnNewMessage;

        IEnumerable<LogItem> LastLogItems { get; }
    }
}