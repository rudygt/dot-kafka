using System;
using System.Collections.Generic;
using DotKafka.Prototype.Common.Config.Types;

namespace DotKafka.Prototype.Common.Config {
    public class ConfigDef {
        private static readonly object NoDefaultValue = string.Empty;
        private readonly Dictionary<string, ConfigKey> _configKeys = new Dictionary<string, ConfigKey>();

        public HashSet<string> Names() {
            return new HashSet<string>(_configKeys.Keys);
        }

        public ConfigDef Define(string name, Type type, object defaultValue, IValidator validator, Importance importance,
            string documentaiton) {
            if (_configKeys.ContainsKey(name))
                throw new ConfigException("Configuration " + name + " is defined twice.");
            object parsedDefault = defaultValue == NoDefaultValue ? NoDefaultValue : ParseType(name, defaultValue, type);

            return new ConfigDef();
        }

        private object ParseType(string name, object value, Type type) {
            try {
                if (value == null)
                    return null;

                string trimmed = null;

                if (value is string)
                    trimmed = ((string) value).Trim();

                switch (type) {
                    case Type.Bool:
                        if (value is string) {
                            if (trimmed.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                                return true;
                            if (trimmed.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                                return false;
                            throw new ConfigException(name, value, "Expected value to be either true or false");
                        }
                        if (value is bool)
                            return value;
                        throw new ConfigException(name, value, "Expected value to be either true or false");
                    case Type.Password:
                        if (value is Password)
                            return value;
                        if (value is string)
                            return new Password(trimmed);
                        throw new ConfigException(name, value, "Expected value to be a string, but it was a " + value.GetType().Name);
                    case Type.String:
                        if (value is string)
                            return trimmed;
                        throw new ConfigException(name, value, "Expected value to be a string, but it was a " + value.GetType().Name);
                    case Type.Int:
                        if (value is int)
                            return (int) value;
                        if (value is string)
                            return int.Parse(trimmed);
                        throw new ConfigException(name, value, "Expected value to be an number.");
                    case Type.Short:
                        if (value is short)
                            return (short) value;
                        if (value is string)
                            return short.Parse(trimmed);
                        throw new ConfigException(name, value, "Expected value to be an number.");
                    case Type.Long:
                        if (value is int)
                            return (long)((int) value);
                        if (value is long)
                            return (long) value;
                        if (value is string)
                            return long.Parse(trimmed);
                        throw new ConfigException(name, value, "Expected value to be an number.");
                    case Type.Double:
                        if (value is double)
                            return (double)value;
                        if (value is string)
                            return double.Parse(trimmed);
                        throw new ConfigException(name, value, "Expected value to be an number.");

                    
                }

            }
            catch (FormatException e) {
                throw new ConfigException(name, value, "Not a number of type " + type, e);
            }

            return new object();
        }

        public enum Type {
            Bool,
            String,
            Int,
            Short,
            Long,
            Double,
            List,
            Class,
            Password
        }

        public enum Importance {
            High,
            Medium,
            Low
        }

        public interface IValidator {
            void EnsureValid(string name, object o);
        }

        private class ConfigKey {
            public string Name { get; set; }
            public Type Type { get; set; }
            public string Documentation { get; set; }
            public object DefaultValue { get; set; }
            public IValidator Validator { get; set; }
            public Importance Importance { get; set; }

            public ConfigKey(string name, Type type, object defaultValue, IValidator validator, Importance importance,
                string documentation) {
                Name = name;
                Type = type;
                DefaultValue = defaultValue;
                Validator = validator;
                Importance = importance;
                Validator?.EnsureValid(name, defaultValue);
                Documentation = documentation;
            }

            public bool HasDefault() {
                return DefaultValue != NoDefaultValue;
            }
        }
    }
}
