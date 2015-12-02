using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class InvalidTopicException : ApiException
    {
        public InvalidTopicException() { }

        public InvalidTopicException(string message) : base(message) { }

        public InvalidTopicException(string message, Exception inner) : base(message, inner) { }
    }
}