using System.Runtime.CompilerServices;

namespace DotKafka.Prototype.Common.Cache
{
    internal class SynchronizedCache<K, V> : ICache<K, V> {

        private readonly ICache<K, V> _underlying;

        public int Size {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return _underlying.Size; }
        }

        public SynchronizedCache(ICache<K, V> underlying) {
            this._underlying = underlying;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public V Get(K key) {
            return _underlying.Get(key);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Put(K key, V value) {
            _underlying.Put(key,value);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Remove(K key) {
            return _underlying.Remove(key);
        }
    }
}