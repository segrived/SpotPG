namespace SpotPG.Services.Abstractions
{
    public interface ISpotifyCredentialsManager
    {
        string ClientId { get; set; }

        string ClientSecret { get; set; }

        string AccessToken { get; set; }

        string RefreshToken { get; set; }
    }
}