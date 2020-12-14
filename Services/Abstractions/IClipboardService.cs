using System.Threading.Tasks;

namespace SpotPG.Services.Abstractions
{
    public interface IClipboardService
    {
        ValueTask<string> ReadTextAsync();
        ValueTask WriteTextAsync(string text);
    }
}