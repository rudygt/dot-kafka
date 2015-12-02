using System;
using System.IO;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class KafkaInt16 : IType
    {
        public object Read(MemoryStream buffer)
        {
            var reader = new BinaryReader(buffer);
            return reader.ReadInt16();
        }

        public int SizeOf(object item)
        {
            return 2;
        }

        public override string ToString()
        {
            return "INT16";
        }

        public object Validate(object item)
        {
            if (!(item is Int16))
            {
                throw new SchemaException(item + " is not a Short");
            }

            return (Int16)item;
        }

        public void Write(MemoryStream buffer, object item)
        {
            var writer = new BinaryWriter(buffer);
            writer.Write((Int16)item);
        }
    }
}