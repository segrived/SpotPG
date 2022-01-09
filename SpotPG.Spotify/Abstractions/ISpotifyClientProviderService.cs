using SpotifyAPI.Web;

namespace SpotPG.Spotify.Abstractions;

public interface ISpotifyClientProviderService
{
    ISpotifyClient GetSpotifyClient();
}