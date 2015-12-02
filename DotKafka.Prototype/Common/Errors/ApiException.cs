using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class ApiException : KafkaException
    {
        public ApiException() { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, Exception inner) : base(message, inner) { }
    }
}