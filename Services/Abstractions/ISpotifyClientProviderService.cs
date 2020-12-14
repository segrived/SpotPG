using SpotifyAPI.Web;

namespace SpotPG.Services.Abstractions
{
    public interface ISpotifyClientProviderService
    {
        ISpotifyClient GetSpotifyClient();
    }
}