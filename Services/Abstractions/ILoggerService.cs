using System;

namespace SpotPG.Services.Abstractions
{
    public interface ILoggerService
    {
        ILogger CreateLogger();

        event EventHandler<LogEventArgs> OnNewMessage;
    }
}