namespace SpotPG.Logger.Abstractions;

public static class LoggerExtensions
{
    public static void LogTrace(this IServiceLogger serviceLogger, string text)
        => serviceLogger.Log(text, LogType.Trace);

    public static void LogInfo(this IServiceLogger serviceLogger, string text)
        => serviceLogger.Log(text, LogType.Info);

    public static void LogWarn(this IServiceLogger serviceLogger, string text)
        => serviceLogger.Log(text, LogType.Warn);

    public static void LogError(this IServiceLogger serviceLogger, string text)
        => serviceLogger.Log(text, LogType.Error);
}