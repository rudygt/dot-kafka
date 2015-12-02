using System.IO;
using System.Linq;
using System.Text;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public interface IType
    {
        void Write(MemoryStream buffer, object item);

        object Read(MemoryStream buffer);

        object Validate(object item);

        int SizeOf(object item);
    }

    public class Struct
    {
        private object[] values;

        private Struct(Schema schema, object[] values)
        {
            Schema = schema;
            this.values = values;
        }

        public Struct(Schema schema)
        {
            Schema = schema;
            values = new object[Schema.NumFields()];
        }

        public Schema Schema { get; }

        private object GetFieldOrDefault(Field field)
        {
            var value = values[field.Index];
            if (value != null)
                return value;
            if (field.DefaultValue != Field.NO_DEFAULT)
                return field.DefaultValue;
            throw new SchemaException("Missing value for field '" + field.Name + "' which has no default value.");
        }

        public object Get(Field field)
        {
            ValidateField(field);
            return GetFieldOrDefault(field);
        }

        public object Get(string name)
        {
            var field = Schema.Get(name);
            if (field == null)
                throw new SchemaException("No such field: " + name);
            return GetFieldOrDefault(field);
        }

        public bool HasField(string name)
        {
            return Schema.Get(name) != null;
        }

        public Struct GetStruct(Field field)
        {
            return (Struct) Get(field);
        }

        public Struct GetStruct(string name)
        {
            return (Struct) Get(name);
        }

        public byte GetByte(Field field)
        {
            return (byte) Get(field);
        }

        public byte GetByte(string name)
        {
            return (byte) Get(name);
        }

        public short GetShort(Field field)
        {
            return (short) Get(field);
        }

        public short GetShort(string name)
        {
            return (short) Get(name);
        }

        public int GetInt(Field field)
        {
            return (int) Get(field);
        }

        public int GetInt(string name)
        {
            return (int) Get(name);
        }

        public long GetInt64(Field field)
        {
            return (long) Get(field);
        }

        public long GetInt64(string name)
        {
            return (long) Get(name);
        }

        public object[] GetArray(Field field)
        {
            return (object[]) Get(field);
        }

        public object[] GetArray(string name)
        {
            return (object[]) Get(name);
        }

        public string Getstring(Field field)
        {
            return (string) Get(field);
        }

        public string Getstring(string name)
        {
            return (string) Get(name);
        }

        private void ValidateField(Field field)
        {
            if (Schema != field.Schema)
                throw new SchemaException("Attempt to access field '" + field.Name +
                                          "' from a different schema instance.");
            if (field.Index > values.Length)
                throw new SchemaException("Invalid field index: " + field.Index);
        }

        public MemoryStream GetBytes(Field field)
        {
            var result = Get(field);
            if (result is byte[])
                return new MemoryStream((byte[]) result);
            return (MemoryStream) result;
        }


        public MemoryStream GetBytes(string name)
        {
            var result = Get(name);
            if (result is byte[])
                return new MemoryStream((byte[]) result);
            return (MemoryStream) result;
        }

        public Struct Set(Field field, object value)
        {
            ValidateField(field);
            values[field.Index] = value;
            return this;
        }

        public Struct Set(string name, object value)
        {
            var field = Schema.Get(name);
            if (field == null)
                throw new SchemaException("Unknown field: " + name);
            values[field.Index] = value;
            return this;
        }

        public Struct Instance(Field field)
        {
            ValidateField(field);
            if (field.Type is Schema)
            {
                return new Struct((Schema) field.Type);
            }
            if (field.Type is KafkaArrayOf)
            {
                var array = (KafkaArrayOf) field.Type;
                return new Struct((Schema) array.Type);
            }
            throw new SchemaException("Field '" + field.Name + "' is not a container type, it is of type " +
                                      field.Type);
        }

        public Struct Instance(string field)
        {
            return Instance(Schema.Get(field));
        }

        public void Clear()
        {
            values = new object[values.Length];
        }

        public int SizeOf()
        {
            return Schema.SizeOf(this);
        }

        public void WriteTo(MemoryStream buffer)
        {
            Schema.Write(buffer, this);
        }

        public void Validate()
        {
            Schema.Validate(this);
        }

        public MemoryStream[] ToBytes()
        {
            var buffer = new MemoryStream(new byte[SizeOf()]);
            WriteTo(buffer);
            return new[] {buffer};
        }

        public override string ToString()
        {
            var b = new StringBuilder();
            b.Append('{');
            for (var i = 0; i < values.Length; i++)
            {
                var f = Schema.Get(i);
                b.Append(f.Name);
                b.Append('=');
                if (f.Type is KafkaArrayOf)
                {
                    var arrayValue = (object[]) values[i];
                    b.Append('[');
                    for (var j = 0; j < arrayValue.Length; j++)
                    {
                        b.Append(arrayValue[j]);
                        if (j < arrayValue.Length - 1)
                            b.Append(',');
                    }
                    b.Append(']');
                }
                else
                    b.Append(values[i]);
                if (i < values.Length - 1)
                    b.Append(',');
            }
            b.Append('}');
            return b.ToString();
        }

        public override int GetHashCode()
        {
            var prime = 31;
            var result = 1;
            for (var i = 0; i < values.Length; i++)
            {
                var f = Schema.Get(i);
                if (f.Type is KafkaArrayOf)
                {
                    var arrayObject = (object[]) Get(f);
                    foreach (var arrayItem in arrayObject)
                        result = prime*result + arrayItem.GetHashCode();
                }
                else
                {
                    result = prime*result + Get(f).GetHashCode();
                }
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            var other = obj as Struct;

            if (other == null)
                return false;

            if (Schema != other.Schema)
                return false;

            for (var i = 0; i < values.Length; i++)
            {
                var f = Schema.Get(i);
                bool result;
                if (f.Type is KafkaArrayOf)
                {
                    result = ((object[]) Get(f)).SequenceEqual((object[]) other.Get(f));
                    //result = Arrays.equals();
                }
                else
                {
                    result = Get(f).Equals(other.Get(f));
                }
                if (!result)
                    return false;
            }
            return true;
        }
    }
}