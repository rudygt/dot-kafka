using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Serialization {

    public class ByteArrayDeserializer : IDeserializer<byte[]> {

        public void Configure(Dictionary<string, object> configs, bool isKey) {

        }

        public byte[] Deserialize(string topic, byte[] data) {
            return data;
        }
    }
}
