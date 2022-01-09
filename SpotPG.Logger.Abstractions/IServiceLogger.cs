namespace SpotPG.Logger.Abstractions;

public interface IServiceLogger
{
    public void Log(string text, LogType type);
}