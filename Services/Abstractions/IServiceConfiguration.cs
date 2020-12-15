using SpotPG.Services.Configuration.Models;

namespace SpotPG.Services.Abstractions
{
    public interface IServiceConfiguration
    {
        SpotifyAccessConfiguration SpotifyAccess { get; }
    }
}