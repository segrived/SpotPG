using System.Threading.Tasks;
using SpotPG.Spotify.Services;

namespace SpotPG.Spotify.Abstractions
{
    public interface ISpotifyConnectionStateProviderService
    {
        Task<SpotifyClientConnectionState> GetSpotifyConnectionStatusAsync();
    }
}