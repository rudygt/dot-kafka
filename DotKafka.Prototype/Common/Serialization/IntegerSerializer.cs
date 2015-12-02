using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Serialization {
    public class IntegerSerializer : ISerializer<int> {

        public void Configure(Dictionary<string, object> configs, bool isKey) {

        }

        public byte[] Serialize(string topic, int data) {

            return new byte[] {
                (byte)(data >> 24),
                (byte)(data >> 16),
                (byte)(data >> 8),
                (byte)data
            };
        }
    }
}
