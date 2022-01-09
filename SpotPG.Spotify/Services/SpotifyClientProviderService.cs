using SpotifyAPI.Web;
using SpotPG.Helpers;
using SpotPG.Logger.Abstractions;
using SpotPG.Models.Configuration;
using SpotPG.Spotify.Abstractions;

namespace SpotPG.Spotify.Services;

public class SpotifyClientProviderService : ISpotifyClientProviderService
{
    private readonly IServiceLogger serviceLogger;
    private readonly SpotifyAccessConfiguration configuration;

    private ISpotifyClient client;
    private SpotifyClientInfo currentInfo;

    public SpotifyClientProviderService(ILoggerService loggerService, SpotifyAccessConfiguration configuration)
    {
        this.serviceLogger = loggerService.CreateLogger();
        this.configuration = configuration;
    }

    public ISpotifyClient GetSpotifyClient()
    {
        var clientInfo = new SpotifyClientInfo(
            EnvHelpers.GetEnvOrDefault("SPOTPG_SPOTIFY_CLIENT_ID", this.configuration.ClientId),
            EnvHelpers.GetEnvOrDefault("SPOTPG_SPOTIFY_CLIENT_SECRET", this.configuration.ClientSecret));

        if (this.client != null && this.currentInfo == clientInfo)
            return this.client;

        this.serviceLogger.LogInfo("Recreation Spotify client...");

        var authResponse = new AuthorizationCodeTokenResponse
        {
            AccessToken = this.configuration.AccessToken,
            RefreshToken = this.configuration.RefreshToken
        };

        var auth = new AuthorizationCodeAuthenticator(clientInfo.Id, clientInfo.Secret, authResponse);

        var config = SpotifyClientConfig.CreateDefault()
            .WithDefaultPaginator(new SimplePaginator())
            .WithRetryHandler(new SimpleRetryHandler())
            .WithAuthenticator(auth);

        this.client = new SpotifyClient(config);
        this.currentInfo = clientInfo;

        this.serviceLogger.LogInfo("New Spotify client was created successfully");

        return this.client;
    }

    private record SpotifyClientInfo(string Id, string Secret);
}