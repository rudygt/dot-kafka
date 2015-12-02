using System;
using DotKafka.Prototype.Common.Errors;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class SchemaException : KafkaException
    {
        public SchemaException() { }

        public SchemaException(string message) : base(message) { }

        public SchemaException(string message, Exception inner) : base(message, inner) { }
    }
}