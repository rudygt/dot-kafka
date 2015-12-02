using System;
using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Errors
{
    public class TopicAuthorizationException : AuthorizationException
    {
        public HashSet<string> Topics { get; set; }

        public TopicAuthorizationException() { }

        public TopicAuthorizationException(string message) : base(message) { }

        public TopicAuthorizationException(string message, Exception inner) : base(message, inner) { }
    }
}