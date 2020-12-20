using System;
using System.Collections.Generic;
using SpotPG.Services.Logger;

namespace SpotPG.Services.Abstractions
{
    public interface ILoggerService
    {
        ILogger CreateLogger();

        event EventHandler<LogEventArgs> OnNewMessage;

        IEnumerable<LogItem> LastLogItems { get; }
    }
}