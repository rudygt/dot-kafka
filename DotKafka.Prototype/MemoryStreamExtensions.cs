using System.IO;

namespace DotKafka.Prototype
{
    public static class MemoryStreamExtensions
    {
        public static long Remaining(this MemoryStream stream)
        {
            return stream.Length - stream.Position;
        }
    }
}