namespace SpotPG.Frontend.Services.Abstractions;

public interface IClipboardService
{
    ValueTask<string> ReadTextAsync();
    ValueTask WriteTextAsync(string text);
}