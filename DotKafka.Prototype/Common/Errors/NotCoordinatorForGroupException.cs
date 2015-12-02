using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class NotCoordinatorForGroupException : RetriableException
    {
        public NotCoordinatorForGroupException() { }

        public NotCoordinatorForGroupException(string message) : base(message) { }

        public NotCoordinatorForGroupException(string message, Exception inner) : base(message, inner) { }
    }
}