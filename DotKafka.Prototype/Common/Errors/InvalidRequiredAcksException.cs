using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class InvalidRequiredAcksException : ApiException
    {
        public InvalidRequiredAcksException() { }

        public InvalidRequiredAcksException(string message) : base(message) { }

        public InvalidRequiredAcksException(string message, Exception inner) : base(message, inner) { }
    }
}