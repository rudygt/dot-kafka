using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class WakeupException : KafkaException
    {
        public WakeupException() { }

        public WakeupException(string message) : base(message) { }

        public WakeupException(string message, Exception inner) : base(message, inner) { }
    }
}