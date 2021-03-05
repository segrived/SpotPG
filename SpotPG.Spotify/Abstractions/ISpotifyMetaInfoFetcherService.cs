using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using SpotPG.Models;

namespace SpotPG.Spotify.Abstractions
{
    public interface ISpotifyMetaInfoFetcherService
    {
        Task<Result<SpotifyAlbum>> FindReleaseAsync(string searchQuery);

        Task<Result<IEnumerable<SpotifyTrack>>> GetAlbumTrackAsync(string albumId);
    }
}