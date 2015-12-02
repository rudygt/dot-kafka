using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class BrokerNotAvailableException : ApiException
    {
        public BrokerNotAvailableException() { }

        public BrokerNotAvailableException(string message) : base(message) { }

        public BrokerNotAvailableException(string message, Exception inner) : base(message, inner) { }
    }
}