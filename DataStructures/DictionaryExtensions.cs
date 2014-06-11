using System.Collections.Generic;

namespace DataStructures
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue defaultValue = default(TValue))
        {
            TValue outValue;
            if (dictionary.TryGetValue(key, out outValue))
            {
                return outValue;
            }

            return defaultValue;
        }
    }
}
