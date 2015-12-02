using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class InterruptException : KafkaException
    {
        public InterruptException() { }

        public InterruptException(string message) : base(message) { }

        public InterruptException(string message, Exception inner) : base(message, inner) { }
    }
}