using System.Collections.Generic;

namespace DotKafka.Prototype.Common.Config {
    public class ConfigDef {
        private static readonly object NoDefaultValue = string.Empty;
        private readonly Dictionary<string, ConfigKey> _configKeys = new Dictionary<string, ConfigKey>();

        public HashSet<string> Names() {
            return new HashSet<string>(_configKeys.Keys);
        }

        public enum Type {
            Boolean,
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
