using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class UnknownMemberIdException : ApiException
    {
        public UnknownMemberIdException() { }

        public UnknownMemberIdException(string message) : base(message) { }

        public UnknownMemberIdException(string message, Exception inner) : base(message, inner) { }
    }
}