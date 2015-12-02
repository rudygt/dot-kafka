using System;
using System.Collections.Generic;
using System.IO;

namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class Schema : IType
    {

        private Field[] _fields;
        private Dictionary<string, Field> _fieldsByName;

        public Schema(params Field[] fs)
        {
            this._fields = new Field[fs.Length];
            this._fieldsByName = new Dictionary<string, Field>();
            for (int i = 0; i < this._fields.Length; i++)
            {
                Field field = fs[i];
                if (_fieldsByName.ContainsKey(field.Name))
                    throw new SchemaException("Schema contains a duplicate field: " + field.Name);
                this._fields[i] = new Field(i, field.Name, field.Type, field.Doc, field.DefaultValue, this);
                this._fieldsByName.Add(fs[i].Name, this._fields[i]);
            }
        }


        public object Read(MemoryStream buffer)
        {
            throw new NotImplementedException();
        }

        public int SizeOf(object item)
        {
            throw new NotImplementedException();
        }

        public object Validate(object item)
        {
            throw new NotImplementedException();
        }

        public void Write(MemoryStream buffer, object item)
        {
            throw new NotImplementedException();
        }

        public int NumFields()
        {
            throw new NotImplementedException();
        }

        public Field Get(string name)
        {
            throw new NotImplementedException();
        }

        public Field Get(int name)
        {
            throw new NotImplementedException();
        }
    }
}