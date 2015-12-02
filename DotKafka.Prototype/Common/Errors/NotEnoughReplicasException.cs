using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class NotEnoughReplicasException : RetriableException 
    {
        public NotEnoughReplicasException() { }

        public NotEnoughReplicasException(string message) : base(message) { }

        public NotEnoughReplicasException(string message, Exception inner) : base(message, inner) { }
    }
}