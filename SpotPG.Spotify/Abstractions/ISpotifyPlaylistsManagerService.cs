using SpotPG.Models;

namespace SpotPG.Spotify.Abstractions;

public interface ISpotifyPlaylistsManagerService
{
    Task<SpotifyPlaylist> CreatePlaylistAsync(string playlistName, IReadOnlyList<SpotifyTrack> tracks, IProgress<int> progress);

    Task<IEnumerable<SpotifyPlaylist>> GetGeneratedPlaylists();

    Task<bool> DeletePlaylistAsync(string playlistId);
}