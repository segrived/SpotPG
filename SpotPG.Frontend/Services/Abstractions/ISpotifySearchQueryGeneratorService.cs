namespace SpotPG.Frontend.Services.Abstractions
{
    public interface ISpotifySearchQueryGeneratorService
    {
        string Generate(ReleaseInfo releaseInfo, QueryGeneratorParameters parameters);
    }
}