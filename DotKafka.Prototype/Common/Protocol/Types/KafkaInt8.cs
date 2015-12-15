using DotKafka.Prototype.Common.Utils;
using System.IO;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class KafkaInt8 : IType
    {
        public object Read(MemoryStream buffer)
        {
            var reader = new BigEndianBinaryReader(buffer);
            return reader.ReadSByte();
        }

        public int SizeOf(object item)
        {
            return 1;
        }

        public override string ToString()
        {
            return "INT8";
        }

        public object Validate(object item)
        {
            if (!(item is sbyte))
            {
                throw new SchemaException(item + " is not a Byte");
            }

            return (sbyte)item;
        }

        public void Write(MemoryStream buffer, object item)
        {
            var writer = new BigEndianBinaryWriter(buffer);
            writer.Write((sbyte)item);
        }
    }
}