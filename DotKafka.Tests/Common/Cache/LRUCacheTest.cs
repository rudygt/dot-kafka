using DotKafka.Prototype.Common.Cache;
using Xunit;

namespace DotKafka.Tests.Common.Cache {

    public class LRUCacheTest {

        [Fact]
        public void TestPutGet() {
            ICache<string, string> cache = new LRUCache<string, string>(4);

            cache.Put("a", "b");
            cache.Put("c", "d");
            cache.Put("e", "f");
            cache.Put("g", "h");

            Assert.Equal(4, cache.Size);

            Assert.Equal("b", cache.Get("a"));
            Assert.Equal("d", cache.Get("c"));
            Assert.Equal("f", cache.Get("e"));
            Assert.Equal("h", cache.Get("g"));
        }

        [Fact]
        public void TestRemove() {
            ICache<string, string> cache = new LRUCache<string, string>(4);

            cache.Put("a", "b");
            cache.Put("c", "d");
            cache.Put("e", "f");

            Assert.Equal(3, cache.Size);
            Assert.Equal(true, cache.Remove("a"));
            Assert.Equal(2, cache.Size);
            Assert.Null(cache.Get("a"));
            Assert.Equal("d", cache.Get("c"));
            Assert.Equal("f", cache.Get("e"));
            Assert.Equal(false, cache.Remove("KeyDoesNotExist"));
            Assert.Equal(true, cache.Remove("c"));
            Assert.Equal(1, cache.Size);
            Assert.Null(cache.Get("c"));
            Assert.Equal("f", cache.Get("e"));
            Assert.Equal(true, cache.Remove("e"));
            Assert.Equal(0, cache.Size);
            Assert.Null(cache.Get("e"));
        }

        [Fact]
        public void TestEviction() {
            ICache<string, string> cache = new LRUCache<string, string>(2);

            cache.Put("a", "b");
            cache.Put("c", "d");
            Assert.Equal(2, cache.Size);

            cache.Put("e", "f");
            Assert.Equal(2, cache.Size);
            Assert.Null(cache.Get("a"));
            Assert.Equal("d", cache.Get("c"));
            Assert.Equal("f", cache.Get("e"));

            cache.Get("c");
            cache.Put("g", "h");
            Assert.Equal(2, cache.Size);
            Assert.Null(cache.Get("e"));
            Assert.Equal("d", cache.Get("c"));
            Assert.Equal("h", cache.Get("g"));
        }
    }
}
