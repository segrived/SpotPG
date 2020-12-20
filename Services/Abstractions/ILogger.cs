using SpotPG.Services.Logger;

namespace SpotPG.Services.Abstractions
{
    public interface ILogger
    {
        public void Log(string text, LogType type);
    }
}