using Microsoft.JSInterop;
using SpotPG.Frontend.Services.Abstractions;

namespace SpotPG.Frontend.Services;

public sealed class ClipboardService : IClipboardService
{
    private readonly IJSRuntime jsRuntime;

    public ClipboardService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public ValueTask<string> ReadTextAsync()
        => this.jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");

    public ValueTask WriteTextAsync(string text)
        => this.jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
}