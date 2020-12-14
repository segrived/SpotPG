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
    // ReSharper disable once InconsistentNaming
    public partial class M2vSource
    {
        private const string BASE_HOST = "http://m2v.ru";

        private readonly HttpClient httpClient;

        public M2vSource()
        {
            this.httpClient = new HttpClient {BaseAddress = new Uri(BASE_HOST)};
        }

        private async Task<IEnumerable<ReleaseInfo>> GetReleasesAsync(DateTime dateParam)
        {
            int page = 1;
            int totalPagesCount = 0;

            var ctx = BrowsingContext.New(Configuration.Default);

            var releases = new List<string>();

            do
            {
                string pageContent = await this.httpClient.GetStringAsync(GenerateUrl(dateParam, page));

                var document = await ctx.OpenAsync(req => req.Content(pageContent));

                if (totalPagesCount == 0)
                    totalPagesCount = GetPagesCount(document);

                var names = document.QuerySelectorAll(".MainTable td a")
                    .Where(l => l.GetAttribute("href").StartsWith("?id="))
                    .Select(l => l.TextContent)
                    // No way to exclude FLAC releases with query
                    .Where(n => !n.Contains("-FLAC-"));

                releases.AddRange(names);
            } while (page++ < totalPagesCount);

            return releases.Select(name => this.ParserService.Parse(name))
                .Where(r => r.IsSuccess)
                .Select(r => r.Value);
        }

        private static int GetPagesCount(IParentNode document)
        {
            var elems = document.QuerySelectorAll("#nav").ToList();

            if (elems.Count <= 0)
                return 1;

            return elems.Select(e => e.TextContent)
                .Select(e => e.Replace(".", "").Replace("[", "").Replace("]", ""))
                .Select(Int32.Parse).Max();
        }

        private static string GenerateUrl(DateTime date, int page)
        {
            string dateFormat = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            return $"?func=part&Part=8&cur_date={dateFormat}&page={page}";
        }
    }
}