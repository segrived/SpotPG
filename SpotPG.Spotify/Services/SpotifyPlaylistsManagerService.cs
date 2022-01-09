using SpotifyAPI.Web;
using SpotPG.Helpers;
using SpotPG.Logger.Abstractions;
using SpotPG.Models;
using SpotPG.Spotify.Abstractions;
using SpotPG.Spotify.Converters;

namespace SpotPG.Spotify.Services;

public class SpotifyPlaylistsManagerService : ISpotifyPlaylistsManagerService
{
    private const string PLAYLIST_MARKET = "[ Generated with SpotPG ]";

    private readonly IServiceLogger serviceLogger;
    private readonly ISpotifyClientProviderService clientProviderService;

    public SpotifyPlaylistsManagerService(ILoggerService loggerService, ISpotifyClientProviderService clientProviderService)
    {
        this.serviceLogger = loggerService.CreateLogger();
        this.clientProviderService = clientProviderService;
    }

    public async Task<SpotifyPlaylist> CreatePlaylistAsync(string playlistName, IReadOnlyList<SpotifyTrack> tracks, IProgress<int> progress)
    {
        var client = this.clientProviderService.GetSpotifyClient();

        this.serviceLogger.LogInfo($"CreatePlaylistAsync: Create playlist request; Playlist name = {playlistName}");

        var user = await client.UserProfile.Current();

        var request = new PlaylistCreateRequest(playlistName) {Description = PLAYLIST_MARKET};
        var playlist = await client.Playlists.Create(user.Id, request);

        this.serviceLogger.LogInfo($"CreatePlaylistAsync: Playlist {playlistName} was created successfully");

        if (playlist.Id == null)
            return null;

        var chunks = tracks.Select(t => t.Uri).ChunkBy(100).ToList();

        this.serviceLogger.LogInfo($"CreatePlaylistAsync: Adding tracks to playlist {playlistName}...");
        foreach ((var chunk, int progressPercent) in chunks.ProgressForEach())
        {
            progress.Report(progressPercent);
            await client.Playlists.AddItems(playlist.Id, new PlaylistAddItemsRequest(chunk));
        }

        this.serviceLogger.LogInfo($"CreatePlaylistAsync: {tracks.Count} track(s) was successfully to playlist {playlistName}");

        var updatedPlaylist = await client.Playlists.Get(playlist.Id);
        return SpotifyApiConverters.Convert(updatedPlaylist);
    }

    public async Task<IEnumerable<SpotifyPlaylist>> GetGeneratedPlaylists()
    {
        var client = this.clientProviderService.GetSpotifyClient();

        var user = await client.UserProfile.Current();

        var firstPlaylistsPagerResult = await client.Playlists.GetUsers(user.Id);
        var allPlaylists = await client.PaginateAll(firstPlaylistsPagerResult);

        return allPlaylists
            .Where(i => i.Description == PLAYLIST_MARKET)
            .Select(SpotifyApiConverters.Convert).ToList();
    }

    public async Task<bool> DeletePlaylistAsync(string playlistId)
    {
        try
        {
            this.serviceLogger.LogInfo($"DeletePlaylistAsync: New delete request; Playlist ID = {playlistId}");

            var client = this.clientProviderService.GetSpotifyClient();
            return await client.Follow.UnfollowPlaylist(playlistId);
        }
        catch
        {
            return false;
        }
    }
}