using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using SpotPG.Frontend.Services;

namespace SpotPG.Frontend.Pages.Components.Sources
{
    public partial class NfoDbSource
    {
        private const string BASE_HOST = "https://nfodb.ru";

        private readonly HttpClient httpClient;

        public NfoDbSource()
        {
            this.httpClient = new HttpClient {BaseAddress = new Uri(BASE_HOST)};
        }

        private async Task<IEnumerable<ReleaseInfo>> GetReleaseNamesAsync(DateTime dateParam, string genreParam)
        {
            string pageContent = await this.httpClient.GetStringAsync(GenerateUrl(dateParam));

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(req => req.Content(pageContent));

            var tableRows = document.QuerySelectorAll("table tbody tr")
                .Where(tr => tr.ChildElementCount == 5)
                .Skip(1) // header
                .ToList();

            var releases = new List<string>();

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var tableRow in tableRows)
            {
                string releaseName = tableRow.Children[1].FirstChild.TextContent;
                string genreName = tableRow.Children[2].FirstChild.TextContent;

                if (genreParam == null || genreParam == genreName)
                    releases.Add(releaseName);
            }

            return releases.Select(name => this.ParserService.Parse(name))
                .Where(r => r.IsSuccess)
                .Select(r => r.Value);
        }

        private static string GenerateUrl(DateTime date)
        {
            string dateFormat = date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            return $"index.php?rlsday={dateFormat}&do_search=Show";
        }

        private static IEnumerable<string> Genres { get; } = new[]
        {
            "A Cappella",
            "Acid Jazz",
            "Acid Punk",
            "Acid",
            "Acoustic",
            "Alt Rock",
            "Alternative",
            "Ambient",
            "Anime",
            "Avantgarde",
            "Ballad",
            "Bass",
            "Beat",
            "Black Metal",
            "Bluegrass",
            "Blues",
            "Booty Bass",
            "Breaks",
            "BritPop",
            "Cabaret",
            "Celtic",
            "Chamber Music",
            "Chanson",
            "Chillout",
            "Chorus",
            "Christian Rap",
            "Christian Rock",
            "Classic Rock",
            "Classical",
            "Club-House",
            "Club",
            "Comedy",
            "Country",
            "CPop",
            "Crossover",
            "Cult",
            "Dance Hall",
            "Dance",
            "Dark Ambient",
            "Darkwave",
            "Death Metal",
            "Disco",
            "Downtempo",
            "Dream",
            "Drum & Bass",
            "Dub",
            "Dubstep",
            "Duet",
            "Easy Listening",
            "Electro Funk",
            "Electro-House",
            "Electronic",
            "Ethnic",
            "Euro-House",
            "Euro-Techno",
            "Eurodance",
            "Experimental",
            "Folk-Rock",
            "Folk",
            "Folklore",
            "Freestyle",
            "Frenchcore",
            "Funk",
            "Fusion",
            "Gabber",
            "Game",
            "Gangsta Rap",
            "Gangsta",
            "Garage",
            "Goa",
            "Gospel",
            "Gothic Rock",
            "Gothic",
            "Grindcore",
            "Grunge",
            "Happy Hardcore",
            "Hard House",
            "Hard Rock",
            "Hard Trance",
            "Hardcore",
            "Hardstyle",
            "Heavy Metal",
            "Hip-Hop",
            "House",
            "Humour",
            "IDM",
            "Indie",
            "Industrial",
            "Instrumental Pop",
            "Instrumental Rock",
            "Instrumental",
            "Jazz",
            "Jazz+Funk",
            "JPop",
            "Jumpstyle",
            "Jungle",
            "Latin",
            "Lo-Fi",
            "Lounge",
            "Makina",
            "Meditative",
            "Metal",
            "Minimal",
            "Musical",
            "National Folk",
            "Native American",
            "New Age",
            "New Wave",
            "Newstyle",
            "Noise",
            "Oldies",
            "Opera",
            "Other",
            "Polka",
            "Pop-Folk",
            "Pop",
            "Post Rock",
            "Progressive House",
            "Progressive Rock",
            "Progressive Trance",
            "Psychedelic Rock",
            "Psychedelic",
            "Punk Rock",
            "Punk",
            "R&amp;B",
            "Rap",
            "Rave",
            "Reggae",
            "Retro",
            "Revival",
            "Rhythmic Soul",
            "Rock &amp; Roll",
            "Rock",
            "Salsa",
            "Samba",
            "Screamo",
            "Ska",
            "Slow Rock",
            "Sonata",
            "Soul",
            "Sound Clip",
            "Soundtrack",
            "Southern Rock",
            "Space",
            "Speech",
            "Speedcore",
            "Swing",
            "Symphonic Rock",
            "Symphony",
            "Synthpop",
            "Tango",
            "Techno-Industrial",
            "Techno",
            "Terror",
            "Thrash Metal",
            "Top 40",
            "Trailer",
            "Trance",
            "Tribal",
            "Trip-Hop",
            "Unknown",
            "Vocal",
            "World",
        };
    }
}