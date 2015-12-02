using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class GroupLoadInProgressException : RetriableException 
    {
        public GroupLoadInProgressException() { }

        public GroupLoadInProgressException(string message) : base(message) { }

        public GroupLoadInProgressException(string message, Exception inner) : base(message, inner) { }
    }
}