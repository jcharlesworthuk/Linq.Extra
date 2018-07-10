using System;
using System.Collections.Generic;

namespace LinqExtra
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value associated with the specified key, or the default value for this type if the key does not exist
        /// </summary>
        /// <param name="dictionary">The source dictionary</param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryGetValue(key, out TValue ret);
            return ret;
        }

        /// <summary>
        /// Gets the value associated with the specified key, or the supplied substitute value if the key does not exist
        /// </summary>
        /// <param name="dictionary">The source dictionary</param>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="substitute">The value to return if the key does not exist</param>
        /// <returns></returns>
        public static TValue GetValueOr<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue substitute)
            where TValue : struct, IEquatable<TValue>
        {
            dictionary.TryGetValue(key, out TValue ret);
            return ret.Equals(default(TValue)) ? substitute : ret;
        }

        /// <summary>
        /// Gets the value associated with the specified key, or the value returned by the substitute method if the key does not exist
        /// </summary>
        /// <param name="dictionary">The source dictionary</param>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="substitute">A function to return a substitute if the key does not exist</param>
        /// <returns></returns>
        public static TValue GetValueOr<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> substitute)
            where TValue : class
        {
            dictionary.TryGetValue(key, out TValue ret);
            return ret ?? substitute();
        }
    }
}
