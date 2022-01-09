using SpotPG.Frontend.Services.Models;

namespace SpotPG.Frontend.Pages.Components.Sources;

public partial class ManualSource
{
    private IEnumerable<ReleaseInfo> Parse(string releasesList)
    {
        return releasesList
            .Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(name => this.ParserService.Parse(name))
            .Where(r => r.IsSuccess)
            .Select(r => r.Value);
    }
}