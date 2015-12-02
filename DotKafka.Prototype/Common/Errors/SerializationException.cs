using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class SerializationException : KafkaException
    {
        public SerializationException() { }

        public SerializationException(string message) : base(message) { }

        public SerializationException(string message, Exception inner) : base(message, inner) { }
    }
}