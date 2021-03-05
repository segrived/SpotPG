using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using SpotPG.Frontend.Services;

namespace SpotPG.Frontend.Pages.Components.Sources
{
    public partial class JunoDownloadSource
    {
        private const int ITEMS_PER_PAGE = 100;
        private const string BASE_HOST = "https://junodownload.com";

        private readonly HttpClient httpClient;

        public JunoDownloadSource()
        {
            this.httpClient = new HttpClient { BaseAddress = new Uri(BASE_HOST) };
        }

        private async Task<IEnumerable<ReleaseInfo>> GetReleaseNamesAsync(int count, string genre)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            if (count % ITEMS_PER_PAGE != 0)
                throw new Exception("Invalid releases count");

            var releases = new List<ReleaseInfo>();

            string genreUrlPart = this.Genres.FirstOrDefault(g => g.Name == genre)?.UrlPart ?? "all";

            for (int i = 1; i <= count / ITEMS_PER_PAGE; i++)
            {
                string url = $"/{genreUrlPart}/this-week/releases/{i}/?order=date_down&items_per_page={ITEMS_PER_PAGE}";
                string pageContent = await this.httpClient.GetStringAsync(url);

                var document = await context.OpenAsync(req => req.Content(pageContent));

                var listingItems = document.QuerySelectorAll(".jd-listing-item");

                // No more releases
                if (listingItems.Length == 0)
                    return releases;

                foreach (var listingItem in listingItems)
                {
                    string artistNames = GetArtists(listingItem);
                    string artistTitle = GetTitle(listingItem);

                    if (!TryGetDate(listingItem, out var date))
                        continue;

                    releases.Add(new ReleaseInfo(artistNames, artistTitle, date.Year));
                }
            }

            return releases;
        }

        private static string GetTitle(IParentNode listingItem)
        {
            var artistTitleLink = listingItem.QuerySelector(".juno-title");
            string artistTitle = artistTitleLink.TextContent;
            return artistTitle;
        }

        private static string GetArtists(IParentNode listingItem)
        {
            var artistLinks = listingItem.QuerySelectorAll(".juno-artist a");
            string artistNames = String.Join(", ", artistLinks.Select(a => a.TextContent));

            if (String.IsNullOrEmpty(artistNames))
            {
                // "Various" as artists case
                artistNames = listingItem.QuerySelector(".juno-artist").TextContent;
            }

            return artistNames;
        }

        private static bool TryGetDate(IParentNode listingItem, out DateTimeOffset date)
        {
            date = DateTimeOffset.MinValue;

            string[] releaseDate = listingItem.QuerySelector(".text-right > .text-sm")
                ?.InnerHtml
                ?.Split("<br>");

            if (releaseDate == null || releaseDate.Length != 3)
                return false;

            date = DateTimeOffset.ParseExact(releaseDate[1], "dd MMM yy", CultureInfo.InvariantCulture);
            return true;
        }

        private IEnumerable<JunoDownloadGenre> Genres { get; } = new List<JunoDownloadGenre>
        {
            new("all", "All Genres"),
            new("downtempo", "Balearic/Downtempo"),
            new("bass", "Bass"),
            new("breakbeat", "Breakbeat"),
            new("disco", "Disco/Nu-Disco"),
            new("dj-tools", "DJ Tools"),
            new("drumandbass", "Drum And Bass"),
            new("deep-dubstep", "Deep Dubstep"),
            new("dirty-heavy-dubstep", "Dirty Dubstep/Trap/Grime"),
            new("electro", "Electro"),
            new("dance-pop", "Euro Dance/Pop Dance"),
            new("footwork-juke", "Footwork/Juke"),
            new("broken-beat", "Broken Beat/Nu Jazz"),
            new("funk-reissues", "Funk"),
            new("international", "International"),
            new("jazz", "Jazz"),
            new("soul", "Soul"),
            new("gabba", "Gabba"),
            new("hardstyle", "Hardstyle"),
            new("uk-hardcore", "UK Hardcore"),
            new("hip-hop", "Hip Hop/R&B"),
            new("deep-house", "Deep House"),
            new("electro-house", "Electro House"),
            new("funky-club-house", "Funky/Club House"),
            new("hard-house", "Hard House"),
            new("minimal-tech-house", "Minimal/Tech House"),
            new("progressive-house", "Progressive House"),
            new("scouse-house", "Scouse House"),
            new("ambient-drone", "Ambient/Drone"),
            new("coldwave-synth", "Coldwave/Synth"),
            new("experimental-electronic", "Experimental/Electronic"),
            new("industrial-noise", "Industrial/Noise"),
            new("soundtrack", "Soundtracks"),
            new("pop", "Pop"),
            new("reggae-classics", "Classics/Ska"),
            new("dancehall-reggae", "Dancehall/Ragga"),
            new("dub-reggae", "Dub"),
            new("roots-reggae", "Roots/Lovers Rock"),
            new("rock-music", "Rock (All)"),
            new("50s-60s-rocknroll-rhythmandblues", "50s/60s"),
            new("indie", "Indie"),
            new("hard-techno", "Hard Techno"),
            new("techno-music", "Techno"),
            new("hard-trance", "Hard Trance"),
            new("pop-trance", "Pop Trance"),
            new("psy-goa-trance", "Psy/Goa Trance"),
            new("uplifting-trance", "Uplifting Trance"),
            new("4x4-garage", "UK Garage")
        };
    }

    public record JunoDownloadGenre(string UrlPart, string Name);
}