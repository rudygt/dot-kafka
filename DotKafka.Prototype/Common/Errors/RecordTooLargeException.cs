using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class RecordTooLargeException : ApiException
    {
        public RecordTooLargeException() { }

        public RecordTooLargeException(string message) : base(message) { }

        public RecordTooLargeException(string message, Exception inner) : base(message, inner) { }
    }
}