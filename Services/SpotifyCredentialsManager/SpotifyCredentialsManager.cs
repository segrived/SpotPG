using LiteDB;
using SpotPG.Services.Abstractions;
using SpotPG.Services.SpotifyCredentialsManager.Models;

namespace SpotPG.Services.SpotifyCredentialsManager
{
    public class SpotifyCredentialsManager : ISpotifyCredentialsManager
    {
        private IDatabaseProviderService Context { get; }

        private ILiteCollection<AppConfigItem> ConfigCollection
            => this.Context.Database.GetCollection<AppConfigItem>("config");

        public SpotifyCredentialsManager(IDatabaseProviderService ctx)
        {
            this.Context = ctx;
            this.ConfigCollection.EnsureIndex(c => c.Key, true);
        }

        public string ClientId
        {
            get => this.GetConfigValue(nameof(this.ClientId));
            set => this.SetConfigValue(nameof(this.ClientId), value);
        }

        public string ClientSecret
        {
            get => this.GetConfigValue(nameof(this.ClientSecret));
            set => this.SetConfigValue(nameof(this.ClientSecret), value);
        }

        public string AccessToken
        {
            get => this.GetConfigValue(nameof(this.AccessToken));
            set => this.SetConfigValue(nameof(this.AccessToken), value);
        }

        public string RefreshToken
        {
            get => this.GetConfigValue(nameof(this.RefreshToken));
            set => this.SetConfigValue(nameof(this.RefreshToken), value);
        }

        private string GetConfigValue(string key)
        {
            var item = this.ConfigCollection.FindOne(c => c.Key == key);
            return item?.Value;
        }

        private void SetConfigValue(string key, string value)
        {
            var item = this.ConfigCollection.FindOne(c => c.Key == key);

            if (item == null)
                this.ConfigCollection.Insert(new AppConfigItem {Key = key, Value = value});
            else
            {
                item.Value = value;
                this.ConfigCollection.Update(item);
            }
        }
    }
}