using System;

namespace DotKafka.Prototype.Common.Errors
{
    public class OffsetMetadataTooLarge : ApiException
    {
        public OffsetMetadataTooLarge() { }

        public OffsetMetadataTooLarge(string message) : base(message) { }

        public OffsetMetadataTooLarge(string message, Exception inner) : base(message, inner) { }
    }
}