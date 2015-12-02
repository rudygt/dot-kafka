using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class UnknownTopicOrPartitionException : InvalidMetadataException
    {
        public UnknownTopicOrPartitionException() { }

        public UnknownTopicOrPartitionException(string message) : base(message) { }

        public UnknownTopicOrPartitionException(string message, Exception inner) : base(message, inner) { }
    }
}