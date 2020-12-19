using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using SpotifyAPI.Web;
using SpotPG.Converters;
using SpotPG.Models;
using SpotPG.Services.Abstractions;
using SpotPG.Services.Logger;

namespace SpotPG.Services
{
    public class SpotifyMetaInfoFetcherService : ISpotifyMetaInfoFetcherService
    {
        private readonly ILogger logger;
        private readonly ISpotifyClientProviderService clientProviderService;

        public SpotifyMetaInfoFetcherService(ILogger logger, ISpotifyClientProviderService clientProviderService)
        {
            this.logger = logger;
            this.clientProviderService = clientProviderService;
        }

        /// <summary>
        /// Return tracks list for specific album
        /// </summary>
        /// <param name="albumId">Album ID</param>
        /// <returns>List of all track from specified album</returns>
        public async Task<Result<IEnumerable<SpotifyTrack>>> GetAlbumTrackAsync(string albumId)
        {
            try
            {
                var client = this.clientProviderService.GetSpotifyClient();

                this.logger.LogTrace($"GetAlbumTrackAsync: New request; Album ID={albumId}");

                var firstPageTracks = await client.Albums.GetTracks(albumId);
                var allTracks = await client.PaginateAll(firstPageTracks);

                this.logger.LogTrace($"GetAlbumTrackAsync: Tracklist loaded ({allTracks.Count} tracks)");

                return allTracks.Select(SpotifyApiConverters.Convert).ToResult();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result<SpotifyAlbum>> FindReleaseAsync(string searchQuery)
        {
            try
            {
                var client = this.clientProviderService.GetSpotifyClient();

                this.logger.LogTrace($"FindReleaseAsync: Find release request, query = {searchQuery}");

                var searchResult = await client.Search.Item(new SearchRequest(SearchRequest.Types.Album, searchQuery));

                if (searchResult.Albums.Items == null)
                {
                    this.logger.LogError("FindReleaseAsync: Bad spotify response");
                    return Result.Fail("Invalid response from Spotify");
                }

                switch (searchResult.Albums.Items.Count)
                {
                    case 0:
                        this.logger.LogWarn($"FindReleaseAsync: No matches found on Spotify for {searchQuery}");
                        return Result.Fail("No matches found on Spotify");
                    case > 2:
                        this.logger.LogWarn($"FindReleaseAsync: Too many matches (> 2) found on Spotify for {searchQuery}");
                        return Result.Fail("Too many matches (> 2) found on Spotify");
                    default:
                    {
                        // TODO: Just use first match for now
                        var item = SpotifyApiConverters.Convert(searchResult.Albums.Items[0]);
                        this.logger.LogWarn($"FindReleaseAsync: Match found {item.ReleaseName} ({item.ReleaseDate:yyyy/MM/dd})");

                        return Result.Ok(item);
                    }
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}