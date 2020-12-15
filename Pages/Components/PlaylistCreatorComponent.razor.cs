using System;
using System.Collections.Generic;
using System.Linq;
using SpotPG.Models;

namespace SpotPG.Pages.Components
{
    public partial class PlaylistCreatorComponent
    {
        private static IList<SpotifyAlbum> ProcessReleases(IList<SpotifyAlbum> input, PlaylistCreationOptions opts)
        {
            var releases = new List<SpotifyAlbum>();

            if (opts.IncludeAlbums)
                releases.AddRange(input.Where(r => r.ReleaseType == ReleaseType.Album));
            if (opts.IncludeCompilations)
                releases.AddRange(input.Where(r => r.ReleaseType == ReleaseType.Compilation));
            if (opts.IncludeSingles)
                releases.AddRange(input.Where(r => r.ReleaseType == ReleaseType.Single));

            return opts.SortBy switch
            {
                SortMode.ByAlbum       => releases.OrderBy(r => String.Join(", ", r.Artists)).ToList(),
                SortMode.ByReleaseName => releases.OrderBy(r => r.ReleaseName).ToList(),
                SortMode.ByReleaseDate => releases.OrderBy(r => r.ReleaseDate).ToList(),
                var _ => releases
            };
        }

        private enum SortMode
        {
            ByAlbum,
            ByReleaseName,
            ByReleaseDate
        }

        private class PlaylistCreationOptions
        {
            public bool IncludeAlbums { get; set; } = true;
            public bool IncludeCompilations { get; set; } = true;
            public bool IncludeSingles { get; set; } = true;

            public SortMode SortBy { get; set; }
        }
    }
}