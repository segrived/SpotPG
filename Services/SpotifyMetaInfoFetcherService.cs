using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using SpotifyAPI.Web;
using SpotPG.Converters;
using SpotPG.Models;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services
{
    public class SpotifyMetaInfoFetcherService : ISpotifyMetaInfoFetcherService
    {
        private readonly ISpotifyClientProviderService clientProviderService;

        public SpotifyMetaInfoFetcherService(ISpotifyClientProviderService clientProviderService)
        {
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

                var firstPageTracks = await client.Albums.GetTracks(albumId);
                var allTracks = await client.PaginateAll(firstPageTracks);

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
                var searchResult = await client.Search.Item(new SearchRequest(SearchRequest.Types.Album, searchQuery));

                if (searchResult.Albums.Items == null)
                    return Result.Fail("Invalid response from Spotify");

                switch (searchResult.Albums.Items.Count)
                {
                    case 0:
                        return Result.Fail("No matches found on Spotify");
                    case > 2:
                        return Result.Fail("Too many matches (> 2) found on Spotify");
                    default:
                    {
                        // TODO: Just use first match for now
                        var item = searchResult.Albums.Items[0];
                        return Result.Ok(SpotifyApiConverters.Convert(item));
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