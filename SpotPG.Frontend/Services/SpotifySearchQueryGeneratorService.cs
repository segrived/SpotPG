﻿using System.Text;
using SpotPG.Frontend.Services.Abstractions;
using SpotPG.Frontend.Services.Models;

namespace SpotPG.Frontend.Services;

public class SpotifySearchQueryGeneratorService : ISpotifySearchQueryGeneratorService
{
    public string Generate(ReleaseInfo releaseInfo, QueryGeneratorParameters parameters)
    {
        (string artists, string title, int year) = releaseInfo;

        var sb = new StringBuilder($"album:{title}");

        if (artists != "VA" && artists != "Various Artists")
            sb.Append($" artist:{artists}");

        if (parameters.UseSpecifiedYear)
            sb.Append($" year:{year}");

        return sb.ToString();
    }
}

public record QueryGeneratorParameters(bool UseSpecifiedYear);