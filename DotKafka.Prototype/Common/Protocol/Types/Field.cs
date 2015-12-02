namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class Field
    {
        public static object NO_DEFAULT = new object();

        public Schema Schema { get; }

        public Field(int index, string name, IType type, string doc, object defaultValue, Schema schema)
        {
            Index = index;
            Name = name;
            Type = type;
            Doc = doc;
            DefaultValue = defaultValue;
            this.Schema = schema;
            if (defaultValue != NO_DEFAULT)
                type.Validate(defaultValue);
        }


        public Field(int index, string name, IType type, string doc, object defaultValue)
            : this(index, name, type, doc, defaultValue, null)
        {
        }

        public Field(string name, IType type, string doc, object defaultValue) : this(-1, name, type, doc, defaultValue)
        {
        }

        public Field(string name, IType type, string doc) : this(name, type, doc, NO_DEFAULT)
        {
        }

        public Field(string name, IType type) : this(name, type, "")
        {
        }

        public string Name { get; private set; }
        public IType Type { get; private set; }
        public object DefaultValue { get; private set; }
        public string Doc { get; private set; }

        public int Index { get; }
    }
}