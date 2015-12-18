using DotKafka.Prototype.Common.Errors;
using DotKafka.Prototype.Common.Protocol.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotKafka.Prototype.Common.Protocol
{
    public class ProtoUtils
    {
        private static Schema SchemaFor(Schema[][] schemas, int apiKey, int version)
        {
            if (apiKey < 0 || apiKey > schemas.Length)
                throw new KafkaException("Invalid api key: " + apiKey);
            Schema[] versions = schemas[apiKey];
            if (version < 0 || version > versions.Length)
                throw new KafkaException("Invalid version for API key " + apiKey + ": " + version);
            if (versions[version] == null)
                throw new KafkaException("Unsupported version for API key " + apiKey + ": " + version);
            return versions[version];
        }

        public static short latestVersion(int apiKey)
        {
            if (apiKey < 0 || apiKey >= Protocol.CURR_VERSION.Length)
                throw new KafkaException("Invalid api key: " + apiKey);
            return Protocol.CURR_VERSION[apiKey];
        }

        public static Schema requestSchema(int apiKey, int version)
        {
            return SchemaFor(Protocol.REQUESTS, apiKey, version);
        }

        public static Schema currentRequestSchema(int apiKey)
        {
            return requestSchema(apiKey, latestVersion(apiKey));
        }

        public static Schema responseSchema(int apiKey, int version)
        {
            return SchemaFor(Protocol.RESPONSES, apiKey, version);
        }

        public static Schema currentResponseSchema(int apiKey)
        {
            return SchemaFor(Protocol.RESPONSES, apiKey, latestVersion(apiKey));
        }

        public static Struct parseRequest(int apiKey, int version, MemoryStream buffer)
        {
            return (Struct)requestSchema(apiKey, version).Read(buffer);
        }

        public static Struct parseResponse(int apiKey, MemoryStream buffer)
        {
            return (Struct)currentResponseSchema(apiKey).Read(buffer);
        }

        public static Struct parseResponse(int apiKey, int version, MemoryStream buffer)
        {
            return (Struct)responseSchema(apiKey, version).Read(buffer);
        }
    }
}
