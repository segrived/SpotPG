using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using FluentResults;
using SpotPG.Frontend.Services;

namespace SpotPG.Frontend.Pages.Components.Sources
{
    public partial class SceneMp3Source
    {
        private const string BASE_HOST = "https://scenemp3.org";

        private readonly HttpClient httpClient;

        public SceneMp3Source()
        {
            this.httpClient = new HttpClient {BaseAddress = new Uri(BASE_HOST)};
        }

        private async Task<Result<IEnumerable<ReleaseInfo>>> GetReleaseNamesAsync(DateTime dateParam)
        {
            string url = GenerateUrl(dateParam);
            string pageContent = await this.httpClient.GetStringAsync(url);

            // Special case: https://github.com/segrived/SpotPG/issues/4
            if (pageContent == "<h1>Establishing a Database Connection</h1>")
                return Result.Fail("scenemp3.org server is not available");

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
                .Select(r => r.Value)
                .ToResult();
        }

        private static string GenerateUrl(DateTime date) => $"date/{date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
    }
}