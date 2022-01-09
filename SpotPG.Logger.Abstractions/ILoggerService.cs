namespace SpotPG.Logger.Abstractions;

public interface ILoggerService
{
    IServiceLogger CreateLogger();

    event EventHandler<LogEventArgs> OnNewMessage;

    IEnumerable<LogItem> LastLogItems { get; }
}