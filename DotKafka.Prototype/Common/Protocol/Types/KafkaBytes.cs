using System;
using System.IO;
using System.Text;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class KafkaBytes : IType
    {
        public object Read(MemoryStream buffer)
        {
            var reader = new BinaryReader(buffer);

            var length = reader.ReadInt32();

            byte[] bytes = new byte[length];

            buffer.Read(bytes, 0, length);

            return new MemoryStream(bytes);
        }

        public int SizeOf(object item)
        {
            return 2 + Encoding.UTF8.GetByteCount((string)item);
        }

        public override string ToString()
        {
            return "BYTES";
        }

        public object Validate(object item)
        {
            if (!(item is MemoryStream))
            {
                throw new SchemaException(item + " is not a MemoryStream");
            }

            return (MemoryStream)item;
        }

        public void Write(MemoryStream buffer, object item)
        {
            var stream = (MemoryStream)item;

            var writer = new BinaryWriter(buffer);
            writer.Write((Int32)stream.Length);
            writer.Write(stream.GetBuffer());
        }
    }
}