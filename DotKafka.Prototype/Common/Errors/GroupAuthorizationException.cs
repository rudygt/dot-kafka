using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class GroupAuthorizationException : AuthorizationException
    {
        public int GroupId { get; set; }
        public GroupAuthorizationException() { }

        public GroupAuthorizationException(string message) : base(message) { }

        public GroupAuthorizationException(string message, Exception inner) : base(message, inner) { }
    }
}