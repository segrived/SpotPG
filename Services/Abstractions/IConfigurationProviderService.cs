using SpotPG.Services.Configuration.Models;

namespace SpotPG.Services.Abstractions
{
    public interface IConfigurationProviderService
    {
        ServiceConfiguration Config { get; }

        void Save();
    }
}