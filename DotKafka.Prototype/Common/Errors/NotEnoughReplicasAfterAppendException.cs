using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class NotEnoughReplicasAfterAppendException : RetriableException
    {
        public NotEnoughReplicasAfterAppendException() { }

        public NotEnoughReplicasAfterAppendException(string message) : base(message) { }

        public NotEnoughReplicasAfterAppendException(string message, Exception inner) : base(message, inner) { }
    }
}