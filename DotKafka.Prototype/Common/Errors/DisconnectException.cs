using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class DisconnectException : RetriableException
    {
        public DisconnectException() { }

        public DisconnectException(string message) : base(message) { }

        public DisconnectException(string message, Exception inner) : base(message, inner) { }
    }
}