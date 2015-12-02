using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class RecordBatchTooLargeException : ApiException
    {
        public RecordBatchTooLargeException() { }

        public RecordBatchTooLargeException(string message) : base(message) { }

        public RecordBatchTooLargeException(string message, Exception inner) : base(message, inner) { }
    }
}