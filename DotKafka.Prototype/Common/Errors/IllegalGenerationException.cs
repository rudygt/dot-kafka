using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class IllegalGenerationException : ApiException
    {
        public IllegalGenerationException() { }

        public IllegalGenerationException(string message) : base(message) { }

        public IllegalGenerationException(string message, Exception inner) : base(message, inner) { }
    }
}