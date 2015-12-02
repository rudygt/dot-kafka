using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class RetriableException : ApiException 
    {
        public RetriableException() { }

        public RetriableException(string message) : base(message) { }

        public RetriableException(string message, Exception inner) : base(message, inner) { }
    }
}