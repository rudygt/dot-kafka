using System;
using System.Collections.Generic;
using System.Text;
using DotKafka.Prototype.Common.Errors;

namespace DotKafka.Prototype.Common.Serialization {

    public class StringSerializer : ISerializer<string> {

        /* https://msdn.microsoft.com/en-us/library/system.text.encodinginfo.displayname(v=vs.110).aspx */
        private string _encodingName = "utf-8";
        private Encoding _encoding;

        public void Configure(Dictionary<string, object> configs, bool isKey) {

            string propertyName = isKey ? "key.serializer.encoding" : "value.serializer.encoding";

            object encodingValue;
            configs.TryGetValue(propertyName, out encodingValue);

            if (encodingValue == null)
                configs.TryGetValue("serializer.encoding", out encodingValue);

            var s = encodingValue as string;

            if (s != null)
                _encodingName = s;

            _encoding = Encoding.GetEncoding(_encodingName);
        }

        public byte[] Serialize(string topic, string data) {
            try {
                return data == null ? null : _encoding.GetBytes(data);
            }
            catch (Exception e) {
                throw new SerializationException( "Error when serializing string to byte[] due to unsupported encoding " + _encodingName, e );
            }
        }
    }
}
