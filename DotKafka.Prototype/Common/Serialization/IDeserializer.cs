using System;
using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Serialization {
    public interface IDeserializer<T> {

        void Configure(Dictionary<string, object> configs, bool isKey);

        T Deserialize(string topic, byte[] data);
    }
}
