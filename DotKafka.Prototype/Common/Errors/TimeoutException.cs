using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class TimeoutException : RetriableException 
    {
        public TimeoutException() { }

        public TimeoutException(string message) : base(message) { }

        public TimeoutException(string message, Exception inner) : base(message, inner) { }
    }
}