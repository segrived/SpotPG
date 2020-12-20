using SpotifyAPI.Web;
using SpotPG.Services.Abstractions;
using SpotPG.Services.Logger;
using SpotPG.Utils;

namespace SpotPG.Services
{
    public class SpotifyClientProviderService : ISpotifyClientProviderService
    {
        private readonly ILogger logger;
        private readonly IServiceConfiguration configuration;

        private ISpotifyClient client;
        private SpotifyClientInfo currentInfo;

        public SpotifyClientProviderService(ILoggerService loggerService, IServiceConfiguration configuration)
        {
            this.logger = loggerService.CreateLogger();
            this.configuration = configuration;
        }

        public ISpotifyClient GetSpotifyClient()
        {
            var clientInfo = new SpotifyClientInfo(
                Helpers.GetEnvOrDefault("SPOTPG_SPOTIFY_CLIENT_ID", this.configuration.SpotifyAccess.ClientId),
                Helpers.GetEnvOrDefault("SPOTPG_SPOTIFY_CLIENT_SECRET", this.configuration.SpotifyAccess.ClientSecret));

            if (this.client != null && this.currentInfo == clientInfo)
                return this.client;

            this.logger.LogInfo("Recreation Spotify client...");

            var authResponse = new AuthorizationCodeTokenResponse
            {
                AccessToken = this.configuration.SpotifyAccess.AccessToken,
                RefreshToken = this.configuration.SpotifyAccess.RefreshToken
            };

            var auth = new AuthorizationCodeAuthenticator(clientInfo.Id, clientInfo.Secret, authResponse);

            var config = SpotifyClientConfig.CreateDefault()
                .WithDefaultPaginator(new SimplePaginator())
                .WithRetryHandler(new SimpleRetryHandler())
                .WithAuthenticator(auth);

            this.client = new SpotifyClient(config);
            this.currentInfo = clientInfo;

            this.logger.LogInfo("New Spotify client was created successfully");

            return this.client;
        }

        public record SpotifyClientInfo(string Id, string Secret);
    }
}