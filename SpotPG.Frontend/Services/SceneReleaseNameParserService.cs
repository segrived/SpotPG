using System.Text.RegularExpressions;
using FluentResults;
using SpotPG.Frontend.Services.Abstractions;
using SpotPG.Frontend.Services.Models;

namespace SpotPG.Frontend.Services;

public class SceneReleaseNameParserService : ISceneReleaseNameParserService
{
    public Result<ReleaseInfo> Parse(string releaseName)
    {
        if (String.IsNullOrWhiteSpace(releaseName))
            return Result.Fail("Empty release name");

        var releaseNameParts = releaseName.Split("-")
            .Where(i => !String.IsNullOrEmpty(i))
            .ToList();

        if (releaseNameParts.Count < 2)
            return Result.Fail("Too short contains at least 2 parts");

        if (Regex.IsMatch(releaseName, @"(SAT|SBD|AUD|CABLE|DVBS|DAB|Rinse-FM)-\d{2}-\d{2}-\d{2}"))
            return Result.Fail("Unsupported release type (SAT or CABLE for example)");

        string yearStr = releaseNameParts
            .LastOrDefault(p => p.Length == 4 && Regex.IsMatch(p, @"[12][\d]{3}"));

        if (yearStr == null || !Int32.TryParse(yearStr, out int releaseYear) || releaseYear < 1970 || releaseYear > DateTime.UtcNow.Year + 2)
            return Result.Fail("Invalid year");

        string artists = ProcessArtistsPart(releaseNameParts[0]);
        string title = ProcessTitlePart(releaseNameParts[1]);

        return new ReleaseInfo(artists, title, releaseYear).ToResult();
    }

    private static string ProcessArtistsPart(string artistsPart) => artistsPart
        .Replace("_", " ")
        .Replace(" and ", ", ", StringComparison.InvariantCultureIgnoreCase)
        .Replace(" Feat. ", ", ", StringComparison.InvariantCultureIgnoreCase)
        .Replace(" feat ", ", ", StringComparison.InvariantCultureIgnoreCase)
        .Replace(" ft. ", ", ", StringComparison.InvariantCultureIgnoreCase)
        .Replace(" ft ", ", ", StringComparison.InvariantCultureIgnoreCase)
        .Replace(" x ", ", ");

    private static string ProcessTitlePart(string artistsPart)
    {
        string title = artistsPart.Replace("_", " ");
        title = Regex.Replace(title, @"\s*(EP|LP)$", "");
        return title;
    }
}