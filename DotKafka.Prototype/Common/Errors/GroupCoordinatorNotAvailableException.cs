using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class GroupCoordinatorNotAvailableException : RetriableException
    {
        public GroupCoordinatorNotAvailableException() { }

        public GroupCoordinatorNotAvailableException(string message) : base(message) { }

        public GroupCoordinatorNotAvailableException(string message, Exception inner) : base(message, inner) { }
    }
}