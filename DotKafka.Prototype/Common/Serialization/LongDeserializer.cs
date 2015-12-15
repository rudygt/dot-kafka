using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotKafka.Prototype.Common.Serialization {

    public class LongDeserializer : IDeserializer<long> {

        public void Configure(Dictionary<string, object> configs, bool isKey) {

        }

        public long Deserialize(string topic, byte[] data) {

            if (data == null)
                return default(long);

            if(data.Length != 8)
                throw new SerializationException("Size of data received by LongDeserializer is not 8");

            long value = 0;

            foreach (byte b in data) {
                value <<= 8;
                value |= b & 0xFF;
            }

            return value;

        }
    }
}
