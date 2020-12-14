using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using SpotPG.Services;

namespace SpotPG.Pages.Components.Sources
{
    public partial class SceneMp3Source
    {
        private const string BASE_HOST = "https://scenemp3.org";

        private readonly HttpClient httpClient;

        public SceneMp3Source()
        {
            this.httpClient = new HttpClient {BaseAddress = new Uri(BASE_HOST)};
        }

        private async Task<IEnumerable<ReleaseInfo>> GetReleaseNamesAsync(DateTime dateParam)
        {
            string url = GenerateUrl(dateParam);
            string pageContent = await this.httpClient.GetStringAsync(url);

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(req => req.Content(pageContent));
            var links = document.GetElementsByTagName("a").Where(LinkSelector);

            static bool LinkSelector(IElement linkElement)
            {
                return linkElement.ClassList.Contains("rowlink")
                       && linkElement.HasAttribute("href")
                       && linkElement.GetAttribute("href").StartsWith("#demo");
            }

            var linksList = links.ToList();

            foreach (var link in linksList)
                link.RemoveChild(link.FirstChild);

            return linksList.Select(l => l.InnerHtml.Trim())
                .Select(name => this.ParserService.Parse(name))
                .Where(r => r.IsSuccess)
                .Select(r => r.Value);
        }

        private static string GenerateUrl(DateTime date) => $"date/{date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
    }
}