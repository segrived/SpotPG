using FluentResults;

namespace SpotPG.Frontend.Services.Abstractions
{
    public interface ISceneReleaseNameParserService
    {
        Result<ReleaseInfo> Parse(string releaseName);
    }
}