using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotKafka.Prototype.Common.Cache {
    public class LRUCache<K, V> : ICache<K, V> {

        private readonly Dictionary<K, V> _cache;
        private readonly LinkedList<K> _lruList;
        private readonly int _capacity;

        public LRUCache(int maxSize) {
            _capacity = maxSize;
            _cache = new Dictionary<K, V>(_capacity);
            _lruList = new LinkedList<K>();
        }

        public V Get(K key) {
            V result;

            _cache.TryGetValue(key, out result);

            return result;
        }

        public void Put(K key, V value) {

            V current;

            if( !_cache.TryGetValue(key, out current) ) {

                _lruList.AddFirst(key);

                _cache.Add(key, value);
            }

            //if(_cache.Count >= _capacity)

        }

        public bool Remove(K key) {

            return _cache.Remove(key);

        }

        public int Size() {

            return _cache.Count;

        }
    }
}
