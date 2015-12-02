using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class CorruptRecordException : RetriableException
    {
        public CorruptRecordException() { }

        public CorruptRecordException(string message) : base(message) { }

        public CorruptRecordException(string message, Exception inner) : base(message, inner) { }
    }
}