using System.Threading.Tasks;
using FluentResults;

namespace SpotPG.Services.Abstractions
{
    public interface ISpotifyConnectionStateProviderService
    {
        Task<SpotifyClientConnectionState> GetSpotifyConnectionStatusAsync();
    }
}