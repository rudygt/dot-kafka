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

        public int Size => _cache.Count;

        public LRUCache(int maxSize) {
            _capacity = maxSize;
            _cache = new Dictionary<K, V>(_capacity);
            _lruList = new LinkedList<K>();
        }

        public V Get(K key) {
            V value;

            if (!_cache.TryGetValue(key, out value))
                return default(V);

            var node = _lruList.Find(key);

            _lruList.Remove(node);

            _lruList.AddFirst(node);

            return value;
        }

        public void Put(K key, V value) {
            V current;

            LinkedListNode<K> node;

            if (_cache.TryGetValue(key, out current)) {
                node = _lruList.Find(key);

                _lruList.Remove(node);

                _lruList.AddFirst(node);
            }
            else {
                if (_cache.Count >= _capacity) {
                    node = _lruList.Last;

                    _cache.Remove(node.Value);

                    _lruList.RemoveLast();
                }

                _lruList.AddFirst(key);

                _cache.Add(key, value);
            }
        }

        public bool Remove(K key) {
            return _cache.Remove(key) && _lruList.Remove(key);
        }
    }
}
