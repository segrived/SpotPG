using SpotPG.Models.Configuration;

namespace SpotPG.Frontend.Services.Abstractions;

public interface IConfigurationProviderService
{
    ServiceConfiguration Config { get; }

    void Save();
}