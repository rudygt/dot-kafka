using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class ControllerMovedException : ApiException
    {
        public ControllerMovedException() { }

        public ControllerMovedException(string message) : base(message) { }

        public ControllerMovedException(string message, Exception inner) : base(message, inner) { }
    }
}