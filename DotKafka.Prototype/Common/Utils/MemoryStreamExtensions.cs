using System.IO;

namespace DotKafka.Prototype.Common.Utils
{
    public static class MemoryStreamExtensions
    {
        public static long Remaining(this MemoryStream stream)
        {
            return stream.Length - stream.Position;
        }
    }
}