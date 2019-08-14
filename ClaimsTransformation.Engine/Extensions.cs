using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ClaimsTransformation.Engine
{
    public static class Extensions
    {
        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            var value = default(TValue);
            return dictionary.TryRemove(key, out value);
        }

        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            var result = new[]
            {
                Enumerable.Empty<T>()
            };
            foreach (var sequence in sequences)
            {
                //Wtf?
                result = (
                    from existing in result
                    from element in sequence
                    select existing.Concat(new[] { element }).ToArray()
                ).ToArray();
            }
            return result;
        }
    }
}
