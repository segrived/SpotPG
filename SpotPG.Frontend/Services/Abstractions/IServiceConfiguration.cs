using SpotPG.Models.Configuration;

namespace SpotPG.Frontend.Services.Abstractions;

public interface IServiceConfiguration
{
    SpotifyAccessConfiguration SpotifyAccess { get; }
}