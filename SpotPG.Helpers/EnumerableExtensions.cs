using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotPG.Helpers
{
    public static class EnumerableExtensions
    {
        public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<IEnumerable<T1>>> func)
            => (await Task.WhenAll(enumeration.Select(func))).SelectMany(s => s);

        public static List<List<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static IEnumerable<IndexItem<T>> IndexForEach<T>(this IList<T> list)
            => list.Select((t, i) => new IndexItem<T>(t, i));

        public static IEnumerable<ProgressItem<T>> ProgressForEach<T>(this IList<T> list)
            => list.Select((t, i) => new ProgressItem<T>(t, (int)(i / (double)list.Count * 100d)));

        public sealed record IndexItem<T>(T Value, int Index);

        public sealed record ProgressItem<T>(T Value, int ProgressPercent);
    }
}