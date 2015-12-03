using System.Collections.Generic;
using DotKafka.Prototype.Common.Serialization;
using Xunit;

namespace DotKafka.Tests.Common.Serialization {

    public class SerializationTest {

        private class SerDeser<T> {
            public ISerializer<T> Serializer { get; set; }
            public IDeserializer<T> Deserializer { get; set; }
        }

        private const string MyTopic = "TestTopic";

        [Fact]
        public void TestStringSerializer() {
            string str = "my test string";

            var encodings = new List<string> {
                "utf-8",
                "utf-16"
            };

            foreach (string encoding in encodings) {
                var serDeser = GetStringSerDeser(encoding);
                ISerializer<string> serializer = serDeser.Serializer;
                IDeserializer<string> deserializer = serDeser.Deserializer;

                Assert.Equal(str,deserializer.Deserialize(MyTopic, serializer.Serialize(MyTopic,str)));
                Assert.Equal(null,deserializer.Deserialize(MyTopic, serializer.Serialize(MyTopic, null)));
            }
        }

        [Fact]
        public void TestIntegerSerializer() {
            int [] integers = new int[] {
                423412424,
                -41243432,
                0,
                23423,
                6234
            };

            ISerializer<int> serializer = new IntegerSerializer();
            IDeserializer<int> deserializer = new IntegerDeserializer();

            foreach (int i in integers) {
                Assert.Equal(i, deserializer.Deserialize(MyTopic,serializer.Serialize(MyTopic,i)));
            }
        }

        [Fact]
        public void TestLongSerializer()
        {
            long[] longs = new long[] {
                42341242432343233,
                -4234124243234323,
                0
            };

            ISerializer<long> serializer = new LongSerializer();
            IDeserializer<long> deserializer = new LongDeserializer();

            foreach (long l in longs)
            {
                Assert.Equal(l, deserializer.Deserialize(MyTopic, serializer.Serialize(MyTopic, l)));
            }
        }


        private SerDeser<string> GetStringSerDeser(string encoder) {
            var serializerConfigs = new Dictionary<string, object> {
                {"key.serializer.encoding", encoder}
            };

            ISerializer<string> serializer = new StringSerializer();
            serializer.Configure(serializerConfigs,true);

            var deSerializerConfigs = new Dictionary<string, object> {
                {"key.deserializer.encoding", encoder}
            };

            IDeserializer<string> deserializer = new StringDeserializer();
            deserializer.Configure(deSerializerConfigs, true);

            return new SerDeser<string> {
                Serializer = serializer,
                Deserializer = deserializer
            };
        }
    }
}
