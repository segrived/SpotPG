namespace SpotPG.Logger.Abstractions
{
    public interface ILogger
    {
        public void Log(string text, LogType type);
    }
}