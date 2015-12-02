using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Serialization {

    public class ByteArraySerializer : ISerializer<byte[]> {

        public void Configure(Dictionary<string, object> configs, bool isKey) {
            
        }

        public byte[] Serialize(string topic, byte[] data) {
            return data;
        }
    }
}
