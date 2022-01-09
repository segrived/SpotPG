using FluentResults;
using SpotPG.Frontend.Services.Models;

namespace SpotPG.Frontend.Services.Abstractions;

public interface ISceneReleaseNameParserService
{
    Result<ReleaseInfo> Parse(string releaseName);
}