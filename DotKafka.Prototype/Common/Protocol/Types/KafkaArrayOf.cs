using System;
using System.IO;
using DotKafka.Prototype.Common.Utils;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class KafkaArrayOf : IType
    {
        public IType Type { get; set; }

        public KafkaArrayOf()
        {
        }

        public KafkaArrayOf(IType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return "ARRAY(" + Type + ")";
        }

        public object Read(MemoryStream buffer)
        {
            var reader = new BigEndianBinaryReader(buffer);
            int size = reader.ReadInt32();
            if (size > buffer.Remaining())
                throw new SchemaException("Error reading array of size " + size + ", only " + buffer.Remaining() + " bytes available");
            object[] objs = new object[size];
            for (int i = 0; i < size; i++)
                objs[i] = Type.Read(buffer);
            return objs;
        }

        public int SizeOf(object item)
        {
            object[] objs = (object[])item;
            int size = 4;
            for (int i = 0; i < objs.Length; i++)
                size += Type.SizeOf(objs[i]);
            return size;
        }

        public object Validate(object item)
        {
            try
            {
                object[] array = (object[])item;
                for (int i = 0; i < array.Length; i++)
                    Type.Validate(array[i]);
                return array;
            }
            catch (Exception e)
            {
                throw new SchemaException("Not an object[].", e);
            }
        }

        public void Write(MemoryStream buffer, object item)
        {
            object[] objs = (object[])item;
            int size = objs.Length;
            var writer = new BigEndianBinaryWriter(buffer);
            writer.Write(size);
            for (int i = 0; i < size; i++)
                Type.Write(buffer, objs[i]);
        }
    }
}