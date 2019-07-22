using System.Collections.Concurrent;

namespace ClaimsTransformation.Engine
{
    public static class Extensions
    {
        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            var value = default(TValue);
            return dictionary.TryRemove(key, out value);
        }
    }
}
