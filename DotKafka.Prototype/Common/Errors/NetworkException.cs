using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class NetworkException : InvalidMetadataException
    {
        public NetworkException() { }

        public NetworkException(string message) : base(message) { }

        public NetworkException(string message, Exception inner) : base(message, inner) { }
    }
}