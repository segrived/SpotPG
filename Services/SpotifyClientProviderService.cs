using SpotifyAPI.Web;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services
{
    public class SpotifyClientProviderService : ISpotifyClientProviderService
    {
        private readonly ISpotifyCredentialsManager credentialsManager;

        private ISpotifyClient client;

        public SpotifyClientProviderService(ISpotifyCredentialsManager credentialsManager)
        {
            this.credentialsManager = credentialsManager;
        }

        public ISpotifyClient GetSpotifyClient()
        {
            if (this.client != null)
                return this.client;

            var authorizationCodeTokenResponse = new AuthorizationCodeTokenResponse
            {
                AccessToken = this.credentialsManager.AccessToken,
                RefreshToken = this.credentialsManager.RefreshToken
            };

            var auth = new AuthorizationCodeAuthenticator(
                this.credentialsManager.ClientId,
                this.credentialsManager.ClientSecret,
                authorizationCodeTokenResponse);

            var config = SpotifyClientConfig.CreateDefault()
                .WithDefaultPaginator(new SimplePaginator())
                .WithRetryHandler(new SimpleRetryHandler())
                .WithAuthenticator(auth);

            this.client = new SpotifyClient(config);

            return this.client;
        }
    }
}