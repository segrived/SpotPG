using SpotPG.Spotify.Abstractions;

namespace SpotPG.Spotify.Services;

public class SpotifyConnectionStateProviderService : ISpotifyConnectionStateProviderService
{
    private readonly ISpotifyClientProviderService spotifyClientProviderService;

    public SpotifyConnectionStateProviderService(ISpotifyClientProviderService spotifyClientProviderService)
    {
        this.spotifyClientProviderService = spotifyClientProviderService;
    }

    public async Task<SpotifyClientConnectionState> GetSpotifyConnectionStatusAsync()
    {
        try
        {
            var client = this.spotifyClientProviderService.GetSpotifyClient();
            var user = await client.UserProfile.Current();

            return !String.IsNullOrEmpty(user.Id)
                ? SpotifyClientConnectionState.Connected
                : SpotifyClientConnectionState.InvalidUser;
        }
        catch (Exception)
        {
            return SpotifyClientConnectionState.ConnectionError;
        }
    }
}

public enum SpotifyClientConnectionState
{
    Connected,
    ConnectionError,
    InvalidUser
}