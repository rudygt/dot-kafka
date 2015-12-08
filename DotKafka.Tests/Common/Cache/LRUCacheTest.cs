using DotKafka.Prototype.Common.Cache;
using Xunit;

namespace DotKafka.Tests.Common.Cache {

    public class LRUCacheTest {

        [Fact]
        public void TestPutGet() {
            ICache<string, string> cache = new LRUCache<string, string>(4);

        }
    }
}
