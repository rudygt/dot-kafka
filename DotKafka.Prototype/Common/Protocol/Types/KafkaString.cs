using DotKafka.Prototype.Common.Utils;
using System;
using System.IO;
using System.Text;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class KafkaString : IType
    {
        public object Read(MemoryStream buffer)
        {
            var reader = new BigEndianBinaryReader(buffer);

            var length = reader.ReadInt16();

            byte[] bytes = new byte[length];

            buffer.Read(bytes, 0, length);

            return Encoding.UTF8.GetString(bytes);
        }

        public int SizeOf(object item)
        {
            return 2 + Encoding.UTF8.GetByteCount((string)item);
        }

        public override string ToString()
        {
            return "STRING";
        }

        public object Validate(object item)
        {
            if (!(item is string))
            {
                throw new SchemaException(item + " is not a String");
            }

            return (string)item;
        }

        public void Write(MemoryStream buffer, object item)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes((string)item);
            if (utf8Bytes.Length > Int16.MaxValue)
            {
                throw new SchemaException("String is longer than the maximum string length.");
            }
            var writer = new BigEndianBinaryWriter(buffer);
            writer.Write((Int16)utf8Bytes.Length);
            writer.Write(utf8Bytes);
        }
    }
}