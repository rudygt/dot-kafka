using System;
using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Serialization
{
    public interface ISerializer<T> {

        void Configure(Dictionary<string, object> configs, bool isKey);

        byte[] Serialize(string topic, T data);

    }
}
