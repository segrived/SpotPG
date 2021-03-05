namespace SpotPG.Logger.Abstractions
{
    public static class LoggerExtensions
    {
        public static void LogTrace(this ILogger logger, string text) => logger.Log(text, LogType.Trace);

        public static void LogInfo(this ILogger logger, string text) => logger.Log(text, LogType.Info);

        public static void LogWarn(this ILogger logger, string text) => logger.Log(text, LogType.Warn);

        public static void LogError(this ILogger logger, string text) => logger.Log(text, LogType.Error);
    }
}