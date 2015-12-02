using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class KafkaException : Exception
    {
        public KafkaException() { }

        public KafkaException(string message) : base(message) { }

        public KafkaException(string message, Exception inner) : base(message, inner) { }
    }
}