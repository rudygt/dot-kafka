using System.Collections.Generic;
using DotKafka.Prototype.Common.Errors;

namespace DotKafka.Prototype.Common.Serialization {

    public class IntegerDeserializer : IDeserializer<int> {

        public void Configure(Dictionary<string, object> configs, bool isKey) {
            
        }

        public int Deserialize(string topic, byte[] data) {

            if (data == null)
                return default(int);

            if(data.Length != 4)
                throw new SerializationException("Size of data received by IntegerDeserializer is not 4");

            int value = 0;

            foreach (byte b in data) {
                value <<= 8;
                value |= b & 0xFF;
            }

            return value;
        }
    }
}
