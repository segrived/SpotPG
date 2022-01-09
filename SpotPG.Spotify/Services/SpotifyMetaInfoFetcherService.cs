using FluentResults;
using SpotifyAPI.Web;
using SpotPG.Logger.Abstractions;
using SpotPG.Models;
using SpotPG.Spotify.Abstractions;
using SpotPG.Spotify.Converters;

namespace SpotPG.Spotify.Services;

public class SpotifyMetaInfoFetcherService : ISpotifyMetaInfoFetcherService
{
    private readonly IServiceLogger serviceLogger;
    private readonly ISpotifyClientProviderService clientProviderService;

    public SpotifyMetaInfoFetcherService(ILoggerService loggerService, ISpotifyClientProviderService clientProviderService)
    {
        this.serviceLogger = loggerService.CreateLogger();
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

            this.serviceLogger.LogTrace($"GetAlbumTrackAsync: New request; Album ID={albumId}");

            var firstPageTracks = await client.Albums.GetTracks(albumId);
            var allTracks = await client.PaginateAll(firstPageTracks);

            this.serviceLogger.LogTrace($"GetAlbumTrackAsync: Tracklist loaded ({allTracks.Count} tracks)");

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

            this.serviceLogger.LogTrace($"FindReleaseAsync: Find release request, query = {searchQuery}");

            var searchResult = await client.Search.Item(new SearchRequest(SearchRequest.Types.Album, searchQuery));

            if (searchResult.Albums.Items == null)
            {
                this.serviceLogger.LogError("FindReleaseAsync: Bad spotify response");
                return Result.Fail("Invalid response from Spotify");
            }

            switch (searchResult.Albums.Items.Count)
            {
                case 0:
                    this.serviceLogger.LogWarn($"FindReleaseAsync: No matches found on Spotify for {searchQuery}");
                    return Result.Fail("No matches found on Spotify");
                case > 2:
                    this.serviceLogger.LogWarn($"FindReleaseAsync: Too many matches (> 2) found on Spotify for {searchQuery}");
                    return Result.Fail("Too many matches (> 2) found on Spotify");
                default:
                {
                    // TODO: Just use first match for now
                    var item = SpotifyApiConverters.Convert(searchResult.Albums.Items[0]);
                    this.serviceLogger.LogTrace($"FindReleaseAsync: Match found {item.ReleaseName} ({item.ReleaseDate:yyyy/MM/dd})");

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