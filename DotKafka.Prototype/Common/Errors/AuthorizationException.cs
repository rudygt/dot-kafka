using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class AuthorizationException :ApiException
    {
        public AuthorizationException() { }

        public AuthorizationException(string message) : base(message) { }

        public AuthorizationException(string message, Exception inner) : base(message, inner) { }
    }
}