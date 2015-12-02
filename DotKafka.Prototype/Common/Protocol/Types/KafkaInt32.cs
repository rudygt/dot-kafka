using System;
using System.IO;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class KafkaInt32 : IType
    {
        public object Read(MemoryStream buffer)
        {
            var reader = new BinaryReader(buffer);
            return reader.ReadInt32();
        }

        public int SizeOf(object item)
        {
            return 4;
        }

        public override string ToString()
        {
            return "INT32";
        }

        public object Validate(object item)
        {
            if (!(item is Int32))
            {
                throw new SchemaException(item + " is not an Integer");
            }

            return (Int32)item;
        }

        public void Write(MemoryStream buffer, object item)
        {
            var writer = new BinaryWriter(buffer);
            writer.Write((Int32)item);
        }
    }
}