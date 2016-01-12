using DotKafka.Prototype.Common.Protocol.Types;
using System.IO;

namespace DotKafka.Prototype.Common.Requests
{
    public class RequestHeader : AbstractRequestResponse
    {
        public static Field ApiKeyField = Protocol.Protocol.REQUEST_HEADER.Get("api_key");
        public static Field ApiVersionField = Protocol.Protocol.REQUEST_HEADER.Get("api_version");
        public static Field ClientIdField = Protocol.Protocol.REQUEST_HEADER.Get("client_id");
        public static Field CorrelationIdField = Protocol.Protocol.REQUEST_HEADER.Get("correlation_id");

        private short apiKey;
        private short apiVersion;
        private string clientId;
        private int correlationId;

        public RequestHeader(Struct header) : base(header)
        {
            apiKey = _struct.GetShort(ApiKeyField);
            apiVersion = _struct.GetShort(ApiVersionField);
            clientId = _struct.GetString(ClientIdField);
            correlationId = _struct.GetInt(CorrelationIdField);
        }

        public RequestHeader(short apiKey, string client, int correlation) : this(apiKey, Protocol.ProtoUtils.latestVersion(apiKey), client, correlation)
        {
        }

        public RequestHeader(short apiKey, short version, string client, int correlation) : base(new Struct(Protocol.Protocol.REQUEST_HEADER))
        {

            _struct.Set(ApiKeyField, apiKey);
            _struct.Set(ApiVersionField, version);
            _struct.Set(ClientIdField, client);
            _struct.Set(CorrelationIdField, correlation);
            this.apiKey = apiKey;
            this.apiVersion = version;
            this.clientId = client;
            this.correlationId = correlation;
        }

        public static RequestHeader Parse(MemoryStream buffer)
        {
            return new RequestHeader((Struct)Protocol.Protocol.REQUEST_HEADER.Read(buffer));
        }
    }
}
