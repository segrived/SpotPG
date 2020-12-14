using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SpotifyAPI.Web;
using SpotPG.Models;

namespace SpotPG.Converters
{
    public static class SpotifyApiConverters
    {
        public static SpotifyTrack Convert(SimpleTrack spotifyTrack) => new()
        {
            Id = spotifyTrack.Id,
            Uri = spotifyTrack.Uri,
            Name = spotifyTrack.Name,
            ExternalUrl = GetExternalUrl(spotifyTrack.ExternalUrls),
            Artists = GetArtists(spotifyTrack.Artists).ToList(),
            Explicit = spotifyTrack.Explicit,
            DiskNumber = spotifyTrack.DiscNumber,
            TrackNumber = spotifyTrack.TrackNumber,
            Duration = TimeSpan.FromMilliseconds(spotifyTrack.DurationMs)
        };

        public static SpotifyTrack Convert(FullTrack spotifyTrack) => new()
        {
            Id = spotifyTrack.Id,
            Uri = spotifyTrack.Uri,
            Name = spotifyTrack.Name,
            ExternalUrl = GetExternalUrl(spotifyTrack.ExternalUrls),
            Artists = GetArtists(spotifyTrack.Artists).ToList(),
            Explicit = spotifyTrack.Explicit,
            DiskNumber = spotifyTrack.DiscNumber,
            TrackNumber = spotifyTrack.TrackNumber,
            Duration = TimeSpan.FromMilliseconds(spotifyTrack.DurationMs)
        };

        public static SpotifyAlbum Convert(SimpleAlbum spotifyAlbum) => new()
        {
            Id = spotifyAlbum.Id,
            Uri = spotifyAlbum.Uri,
            ReleaseName = spotifyAlbum.Name,
            Artists = GetArtists(spotifyAlbum.Artists).ToList(),
            CoverUrl = GetCoverUrl(spotifyAlbum.Images),
            ExternalUrl = GetExternalUrl(spotifyAlbum.ExternalUrls),
            ReleaseType = GetAlbumType(spotifyAlbum.AlbumType),
            ReleaseDate = GetReleaseDate(spotifyAlbum.ReleaseDate)
        };

        public static SpotifyPlaylist Convert(FullPlaylist spotifyPlaylist) => new()
        {
            Id = spotifyPlaylist.Id,
            Uri = spotifyPlaylist.Uri,
            Name = spotifyPlaylist.Name,
            Description = spotifyPlaylist.Description,
            CoverUrl = GetCoverUrl(spotifyPlaylist.Images),
            IsPublic = spotifyPlaylist.Public,
            ExternalUrl = GetExternalUrl(spotifyPlaylist.ExternalUrls),
            Tracks = GetPlaylistTracks(spotifyPlaylist.Tracks?.Items)
        };

        public static SpotifyPlaylist Convert(SimplePlaylist spotifyPlaylist) => new()
        {
            Id = spotifyPlaylist.Id,
            Uri = spotifyPlaylist.Uri,
            Name = spotifyPlaylist.Name,
            Description = spotifyPlaylist.Description,
            CoverUrl = GetCoverUrl(spotifyPlaylist.Images),
            IsPublic = spotifyPlaylist.Public,
            ExternalUrl = GetExternalUrl(spotifyPlaylist.ExternalUrls),
            Tracks = GetPlaylistTracks(spotifyPlaylist.Tracks.Items)
        };

        private static IEnumerable<SpotifyTrack> GetPlaylistTracks(IEnumerable<PlaylistTrack<IPlayableItem>> tracks)
            => tracks?.Select(i => i.Track).OfType<FullTrack>().Select(Convert) ?? Enumerable.Empty<SpotifyTrack>();

        private static IEnumerable<string> GetArtists(IEnumerable<SimpleArtist> artists)
            => artists.Select(a => a.Name).ToList();

        private static string GetCoverUrl(IEnumerable<Image> covers)
            => covers.FirstOrDefault()?.Url ?? "";

        private static DateTime GetReleaseDate(string releaseDateString)
        {
            // New format (year-month-day)
            var match = Regex.Match(releaseDateString, @"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})");
            if (match.Success)
            {
                int year = Int32.Parse(match.Groups["year"].Value);
                int month = Int32.Parse(match.Groups["month"].Value);
                int day = Int32.Parse(match.Groups["day"].Value);
                return new DateTime(year, month, day);
            }

            // Old format (year only)
            match = Regex.Match(releaseDateString, @"(?<year>\d{4})");
            if (match.Success)
            {
                int year = Int32.Parse(match.Groups["year"].Value);
                return new DateTime(year);
            }

            return DateTime.MinValue;
        }

        private static string GetExternalUrl(IDictionary<string, string> externalUrls)
            => externalUrls.FirstOrDefault(u => u.Key == "spotify").Value ?? "";

        private static ReleaseType GetAlbumType(string albumTypeString) => albumTypeString switch
        {
            "album" => ReleaseType.Album,
            "compilation" => ReleaseType.Compilation,
            "single" => ReleaseType.Single,
            var _ => ReleaseType.Unknown
        };
    }
}