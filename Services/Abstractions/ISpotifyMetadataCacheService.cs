using SpotPG.Models;

namespace SpotPG.Services.Abstractions
{
    public interface ISpotifyMetadataCacheService
    {
        bool TryGetAlbum(string id, out SpotifyAlbum spotifyAlbum);

        bool CacheAlbum(SpotifyAlbum spotifyAlbum);

        bool TryGetTrack(string id, out SpotifyTrack spotifyTrack);

        bool CacheTrack(SpotifyTrack spotifyTrack);
    }
}