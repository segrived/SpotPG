namespace SpotPG.Logger.Abstractions;

public record LogItem(string Text, LogType Type, DateTimeOffset Date);