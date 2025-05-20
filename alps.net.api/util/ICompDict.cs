using System.Collections.Generic;

namespace alps.net.api.util
{
    public interface ICompDict<K, V> : IDictionary<K, V>
    {
        public new bool TryAdd(K key, V value);
    }
}
