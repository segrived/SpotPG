using System.Text;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services
{
    public class SpotifySearchQueryGeneratorService : ISpotifySearchQueryGeneratorService
    {
        public string Generate(ReleaseInfo releaseInfo, QueryGeneratorParameters parameters)
        {
            var sb = new StringBuilder($"album:{releaseInfo.Title}");

            if (releaseInfo.Artists != "VA" && releaseInfo.Artists != "Various Artists")
                sb.Append($" artist:{releaseInfo.Artists}");

            if (parameters.UseSpecifiedYear)
                sb.Append($" year:{releaseInfo.Year}");

            return sb.ToString();
        }
    }

    public record QueryGeneratorParameters(bool UseSpecifiedYear);
}