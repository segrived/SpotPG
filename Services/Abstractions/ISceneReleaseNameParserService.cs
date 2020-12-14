using FluentResults;

namespace SpotPG.Services.Abstractions
{
    public interface ISceneReleaseNameParserService
    {
        Result<ReleaseInfo> Parse(string releaseName);
    }
}