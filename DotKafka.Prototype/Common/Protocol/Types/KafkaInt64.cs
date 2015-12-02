using System;
using System.IO;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class KafkaInt64 : IType
    {
        public object Read(MemoryStream buffer)
        {
            var reader = new BinaryReader(buffer);
            return reader.ReadInt64();
        }

        public int SizeOf(object item)
        {
            return 8;
        }

        public override string ToString()
        {
            return "INT64";
        }

        public object Validate(object item)
        {
            if (!(item is Int64))
            {
                throw new SchemaException(item + " is not a Long");
            }

            return (Int64)item;
        }

        public void Write(MemoryStream buffer, object item)
        {
            var writer = new BinaryWriter(buffer);
            writer.Write((Int64)item);
        }
    }
}