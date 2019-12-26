using Simple.Common;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            ArgCheck.NotNull(nameof(dictionary), dictionary);
            if (dictionary.TryGetValue(key, out var value))
                return value;
            return defaultValue;
        }
    }
}
