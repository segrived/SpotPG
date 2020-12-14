using SpotPG.Models;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services
{
    public class SpotifyMetadataCacheService : ISpotifyMetadataCacheService
    {
        private readonly IDatabaseProviderService databaseProviderService;

        public SpotifyMetadataCacheService(IDatabaseProviderService databaseProviderService)
        {
            this.databaseProviderService = databaseProviderService;
        }

        public bool TryGetAlbum(string id, out SpotifyAlbum spotifyAlbum) => this.TryGetItem(id, out spotifyAlbum);
        public bool CacheAlbum(SpotifyAlbum spotifyAlbum) => this.CacheItem(spotifyAlbum);

        public bool TryGetTrack(string id, out SpotifyTrack spotifyTrack) => this.TryGetItem(id, out spotifyTrack);
        public bool CacheTrack(SpotifyTrack spotifyTrack) => this.CacheItem(spotifyTrack);

        private bool TryGetItem<T>(string id, out T item) where T : class
        {
            item = default;

            var dbItem = this.databaseProviderService.Database.GetCollection<T>().FindById(id);

            if (dbItem == null)
                return false;

            item = dbItem;
            return true;
        }

        private bool CacheItem<T>(T item) => this.databaseProviderService.Database.GetCollection<T>().Upsert(item);
    }
}