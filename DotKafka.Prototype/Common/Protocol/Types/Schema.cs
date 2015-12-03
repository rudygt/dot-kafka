using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            object[] objects = new object[_fields.Length];
            for (int i = 0; i < _fields.Length; i++)
            {
                try
                {
                    objects[i] = _fields[i].Type.Read(buffer);
                }
                catch (Exception e)
                {
                    throw new SchemaException("Error reading field '" + _fields[i].Name + "'", e);
                }
            }
            return new Struct(this, objects);
        }

        public int SizeOf(object item)
        {
            int size = 0;
            Struct r = (Struct)item;
            for (int i = 0; i < _fields.Length; i++)
                size += _fields[i].Type.SizeOf(r.Get(_fields[i]));
            return size;
        }

        public object Validate(object item)
        {
            try
            {
                Struct s = (Struct)item;
                for (int i = 0; i < this._fields.Length; i++)
                {
                    Field field = this._fields[i];
                    try
                    {
                        field.Type.Validate(s.Get(field));
                    }
                    catch (SchemaException e)
                    {
                        throw new SchemaException("Invalid value for field '" + field.Name + "'", e);
                    }
                }
                return s;
            }
            catch (Exception e)
            {
                throw new SchemaException("Not a Struct.", e);
            }
        }

        public void Write(MemoryStream buffer, object item)
        {
            Struct r = (Struct)item;
            for (int i = 0; i < _fields.Length; i++)
            {
                Field f = _fields[i];
                try
                {
                    object value = f.Type.Validate(r.Get(f));
                    f.Type.Write(buffer, value);
                }
                catch (Exception e)
                {
                    throw new SchemaException("Error writing field '" + f.Name + "'", e);
                }
            }
        }

        public int NumFields()
        {
            return this._fields.Length;
        }

        public Field Get(String name)
        {
            return this._fieldsByName[name];
        }

        public Field Get(int slot)
        {
            return this._fields[slot];
        }

        public Field[] Fields()
        {
            return this._fields;
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append('{');
            for (int i = 0; i < this._fields.Length; i++)
            {
                b.Append(this._fields[i].Name);
                b.Append(':');
                b.Append(this._fields[i].Type);
                if (i < this._fields.Length - 1)
                    b.Append(',');
            }
            b.Append("}");
            return b.ToString();
        }

    }
}