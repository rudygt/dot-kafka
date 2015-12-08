namespace DotKafka.Prototype.Common.Cache {

    public interface ICache<K, V> {

        V Get(K key);

        void Put(K key, V value);

        bool Remove(K key);

        int Size();
    }
}
