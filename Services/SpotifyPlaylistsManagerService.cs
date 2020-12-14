using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotPG.Converters;
using SpotPG.Models;
using SpotPG.Services.Abstractions;
using SpotPG.Utils;

namespace SpotPG.Services
{
    public class SpotifyPlaylistsManagerService : ISpotifyPlaylistsManagerService
    {
        private const string PLAYLIST_MARKET = "[ Generated with SpotPG ]";

        private readonly ISpotifyClientProviderService clientProviderService;

        public SpotifyPlaylistsManagerService(ISpotifyClientProviderService clientProviderService)
        {
            this.clientProviderService = clientProviderService;
        }

        public async Task<SpotifyPlaylist> CreatePlaylistAsync(string playlistName, IEnumerable<SpotifyTrack> tracks, IProgress<int> progress)
        {
            var client = this.clientProviderService.GetSpotifyClient();

            var user = await client.UserProfile.Current();

            var request = new PlaylistCreateRequest(playlistName) {Description = PLAYLIST_MARKET};
            var playlist = await client.Playlists.Create(user.Id, request);

            if (playlist.Id == null)
                return null;

            var chunks = tracks.Select(t => t.Uri).ChunkBy(100).ToList();

            foreach ((var chunk, int progressPercent) in chunks.ProgressForEach())
            {
                progress.Report(progressPercent);
                await client.Playlists.AddItems(playlist.Id, new PlaylistAddItemsRequest(chunk));
            }

            var updatedPlaylist = await client.Playlists.Get(playlist.Id);
            return SpotifyApiConverters.Convert(updatedPlaylist);
        }

        public async Task<IEnumerable<SpotifyPlaylist>> GetGeneratedPlaylists()
        {
            var client = this.clientProviderService.GetSpotifyClient();

            var user = await client.UserProfile.Current();
            var playlistsResult = await client.Playlists.GetUsers(user.Id);
            return playlistsResult.Items?.Where(i => i.Description == PLAYLIST_MARKET).Select(SpotifyApiConverters.Convert).ToList();
        }

        public async Task<bool> DeletePlaylistAsync(string playlistId)
        {
            try
            {
                var client = this.clientProviderService.GetSpotifyClient();
                return await client.Follow.UnfollowPlaylist(playlistId);
            }
            catch
            {
                return false;
            }
        }
    }
}