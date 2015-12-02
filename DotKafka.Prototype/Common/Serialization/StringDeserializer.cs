using System;
using System.Collections.Generic;
using System.Text;
using DotKafka.Prototype.Common.Errors;

namespace DotKafka.Prototype.Common.Serialization {

    public class StringDeserializer : IDeserializer<string> {

        /* https://msdn.microsoft.com/en-us/library/system.text.encodinginfo.displayname(v=vs.110).aspx */
        private string _encodingName = "utf-8";
        private Encoding _encoding;

        public void Configure(Dictionary<string, object> configs, bool isKey)
        {
            string propertyName = isKey ? "key.deserializer.encoding" : "value.deserializer.encoding";

            object encodingValue;
            configs.TryGetValue(propertyName, out encodingValue);

            if (encodingValue == null)
                configs.TryGetValue("deserializer.encoding", out encodingValue);

            var s = encodingValue as string;

            if (s != null)
                _encodingName = s;

            _encoding = Encoding.GetEncoding(_encodingName);
        }

        public string Deserialize(string topic, byte[] data) {
            try {
                return data == null ? null : _encoding.GetString(data);
            }
            catch (Exception e) {
                throw new SerializationException( "Error when deserializing byte[] to string due to unsupported encoding " + _encodingName, e );
            }
        }
    }
}
