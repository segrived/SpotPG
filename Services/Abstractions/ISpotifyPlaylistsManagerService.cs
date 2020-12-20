using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpotPG.Models;

namespace SpotPG.Services.Abstractions
{
    public interface ISpotifyPlaylistsManagerService
    {
        Task<SpotifyPlaylist> CreatePlaylistAsync(string playlistName, IReadOnlyList<SpotifyTrack> tracks, IProgress<int> progress);

        Task<IEnumerable<SpotifyPlaylist>> GetGeneratedPlaylists();

        Task<bool> DeletePlaylistAsync(string playlistId);
    }
}