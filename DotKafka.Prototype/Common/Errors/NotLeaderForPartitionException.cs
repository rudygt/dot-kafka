using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class NotLeaderForPartitionException : InvalidMetadataException
    {
        public NotLeaderForPartitionException() { }

        public NotLeaderForPartitionException(string message) : base(message) { }

        public NotLeaderForPartitionException(string message, Exception inner) : base(message, inner) { }
    }
}