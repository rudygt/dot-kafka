using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class InvalidMetadataException : RetriableException
    {
        public InvalidMetadataException() { }

        public InvalidMetadataException(string message) : base(message) { }

        public InvalidMetadataException(string message, Exception inner) : base(message, inner) { }
    }
}