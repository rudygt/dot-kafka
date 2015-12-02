using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class RebalanceInProgressException : ApiException
    {
        public RebalanceInProgressException() { }

        public RebalanceInProgressException(string message) : base(message) { }

        public RebalanceInProgressException(string message, Exception inner) : base(message, inner) { }
    }
}