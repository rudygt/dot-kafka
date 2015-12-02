using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class LeaderNotAvailableException : InvalidMetadataException
    {
        public LeaderNotAvailableException() { }

        public LeaderNotAvailableException(string message) : base(message) { }

        public LeaderNotAvailableException(string message, Exception inner) : base(message, inner) { }
    }
}