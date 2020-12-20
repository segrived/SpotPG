﻿using System.Threading.Tasks;
using Microsoft.JSInterop;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services
{
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

}