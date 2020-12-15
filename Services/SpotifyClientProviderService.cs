using SpotifyAPI.Web;
using SpotPG.Services.Abstractions;
using SpotPG.Utils;

namespace SpotPG.Services
{
    public class SpotifyClientProviderService : ISpotifyClientProviderService
    {
        private readonly IServiceConfiguration configuration;

        private ISpotifyClient client;
        private SpotifyClientInfo currentInfo;

        public SpotifyClientProviderService(IServiceConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ISpotifyClient GetSpotifyClient()
        {
            var clientInfo = new SpotifyClientInfo(
                Helpers.GetEnvOrDefault("SPOTPG_SPOTIFY_CLIENT_ID", this.configuration.SpotifyAccess.ClientId),
                Helpers.GetEnvOrDefault("SPOTPG_SPOTIFY_CLIENT_SECRET", this.configuration.SpotifyAccess.ClientSecret));

            if (this.client != null && this.currentInfo == clientInfo)
                return this.client;

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

            return this.client;
        }

        public record SpotifyClientInfo(string Id, string Secret);
    }
}