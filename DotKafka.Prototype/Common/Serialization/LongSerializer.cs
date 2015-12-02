using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Serialization {

    public class LongSerializer : ISerializer<long> {

        public void Configure(Dictionary<string, object> configs, bool isKey) {

        }

        public byte[] Serialize(string topic, long data) {

            return new byte[] {
                (byte) (data >> 56),
                (byte) (data >> 48),
                (byte) (data >> 40),
                (byte) (data >> 32),
                (byte) (data >> 24),
                (byte) (data >> 16),
                (byte) (data >> 8),
                (byte) data
            };

        }
    }
}
