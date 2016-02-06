using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotKafka.Prototype.Common.Config.Types;

namespace DotKafka.Prototype.Common.Config {
    public class ConfigDef {
        private static readonly object NoDefaultValue = string.Empty;
        private readonly Dictionary<string, ConfigKey> _configKeys = new Dictionary<string, ConfigKey>();

        public HashSet<string> Names() {
            return new HashSet<string>(_configKeys.Keys);
        }

        public ConfigDef Define(string name, Type type, object defaultValue, IValidator validator, Importance importance,
            string documentation) {
            if (_configKeys.ContainsKey(name))
                throw new ConfigException("Configuration " + name + " is defined twice.");
            var parsedDefault = defaultValue == NoDefaultValue ? NoDefaultValue : ParseType(name, defaultValue, type);
            _configKeys.Add(name, new ConfigKey(name, type, parsedDefault, validator, importance, documentation));
            return this;
        }

        public ConfigDef Define(string name, Type type, object defaultValue, Importance importance, string documentation) {
            return Define(name, type, defaultValue, null, importance, documentation);
        }

        public ConfigDef Define(string name, Type type, Importance importance, string documentation) {
            return Define(name, type, NoDefaultValue, null, importance, documentation);
        }

        public ConfigDef WithClientSslSupport() {
            SslConfigs.AddClientSslSupport(this);
            return this;
        }

        public ConfigDef WithClientSaslSupport() {
            SaslConfigs.AddClientSaslSupport(this);
            return this;
        }

        public Dictionary<string, object> Parse(Dictionary<string, object> props) {
            var values = new Dictionary<string, object>();

            foreach (var key in _configKeys.Values) {
                object value;
                if (props.ContainsKey(key.Name))
                    value = ParseType(key.Name, props[key.Name], key.Type);
                else if (key.DefaultValue == NoDefaultValue)
                    throw new ConfigException("Missing required configuration \"" + key.Name +
                                              "\" which has no default value.");
                else
                    value = key.DefaultValue;
                if (key.Validator != null)
                    key.Validator.EnsureValid(key.Name, value);
                values.Add(key.Name, value);
            }

            return values;
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
                        throw new ConfigException(name, value,
                            "Expected value to be a string, but it was a " + value.GetType().Name);
                    case Type.String:
                        if (value is string)
                            return trimmed;
                        throw new ConfigException(name, value,
                            "Expected value to be a string, but it was a " + value.GetType().Name);
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
                            return (long) ((int) value);
                        if (value is long)
                            return (long) value;
                        if (value is string)
                            return long.Parse(trimmed);
                        throw new ConfigException(name, value, "Expected value to be an number.");
                    case Type.Double:
                        if (value is double)
                            return (double) value;
                        if (value is string)
                            return double.Parse(trimmed);
                        throw new ConfigException(name, value, "Expected value to be an number.");
                    case Type.List:
                        if (value is IList)
                            return (IList) value;
                        if (value is string)
                            if (string.IsNullOrEmpty(trimmed))
                                return new List<object>();
                            else
                                return Regex.Split(trimmed, "\\s*,\\s*", RegexOptions.Compiled).ToList();
                        throw new ConfigException(name, value, "Expected a separated comma list.");
                    case Type.Class:
                        throw new NotImplementedException();
                    default:
                        throw new InvalidOperationException();
                }
            }
            catch (FormatException e) {
                throw new ConfigException(name, value, "Not a number of type " + type, e);
            }
            catch (TypeLoadException e) {
                throw new ConfigException(name, value, "Class " + value + " could not be found.");
            }
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

        public class Range : IValidator {
            private double? _min;
            private double? _max;

            private Range(double? min, double? max) {
                _min = min;
                _max = max;
            }

            public static Range AtLeast(double min) {
                return new Range(min, null);
            }

            public static Range Between(double min, double max) {
                return new Range(min, max);
            }

            public void EnsureValid(string name, object o) {
                var n = Convert.ToDouble(o);
                if (_min != null && n < _min.Value)
                    throw new ConfigException(name, o, "Value must be at least " + _min);
                if (_max != null && n > _max.Value)
                    throw new ConfigException(name, o, "Value must be no more than " + _max);
            }

            public override string ToString() {
                if (_min == null)
                    return "[...," + _max + "]";
                if (_max == null)
                    return "[" + _min + ",...]";
                return "[" + _min + ",...," + _max + "]";
            }
        }

        public class ValidString : IValidator {
            private readonly List<string> _validStrings;

            private ValidString(List<string> validStrings) {
                _validStrings = validStrings;
            }

            public ValidString(params string[] validStrings) {
                _validStrings = validStrings.ToList();
            }

            public void EnsureValid(string name, object o) {
                var s = (string) o;
                if (!_validStrings.Contains(s)) {
                    throw new ConfigException(name, o, "String must be one of: " + string.Join(", ", _validStrings));
                }
            }

            public override string ToString() {
                return "[" + string.Join(", ", _validStrings) + "]";
            }
        }

        private class ConfigKey : IComparable {
            public string Name { get; }
            public Type Type { get; }
            public string Documentation { get; }
            public object DefaultValue { get; }
            public IValidator Validator { get; }
            public Importance Importance { get; }

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

            public int CompareTo(object obj) {
                var key = (ConfigKey) obj;
                if (!HasDefault() && key.HasDefault())
                    return -1;
                if (!key.HasDefault() && HasDefault())
                    return 1;

                var cmp = Importance.CompareTo(key.Importance);

                return cmp == 0 ? string.Compare(Name, key.Name, StringComparison.Ordinal) : cmp;
            }
        }

        public string ToHtmlTable() {
            var configs = new List<ConfigKey>(_configKeys.Values);

            configs.Sort();

            var b = new StringBuilder();

            b.Append("<table class=\"data-table\"><tbody>\n");
            b.Append("<tr>\n");
            b.Append("<th>Name</th>\n");
            b.Append("<th>Description</th>\n");
            b.Append("<th>Type</th>\n");
            b.Append("<th>Default</th>\n");
            b.Append("<th>Valid Values</th>\n");
            b.Append("<th>Importance</th>\n");
            b.Append("</tr>\n");
            foreach (var def in configs) {
                b.Append("<tr>\n");
                b.Append("<td>");
                b.Append(def.Name);
                b.Append("</td>");
                b.Append("<td>");
                b.Append(def.Documentation);
                b.Append("</td>");
                b.Append("<td>");
                b.Append(def.Type.ToString().ToLower());
                b.Append("</td>");
                b.Append("<td>");
                if (def.HasDefault()) {
                    if (def.DefaultValue == null)
                        b.Append("null");
                    else if (def.Type == Type.String && def.DefaultValue.ToString() == string.Empty)
                        b.Append("\"\"");
                    else
                        b.Append(def.DefaultValue);
                }
                else
                    b.Append("");
                b.Append("</td>");
                b.Append("<td>");
                b.Append(def.Validator != null ? def.Validator.ToString() : "");
                b.Append("</td>");
                b.Append("<td>");
                b.Append(def.Importance.ToString().ToLower());
                b.Append("</td>");
                b.Append("</tr>\n");
            }
            b.Append("</tbody></table>");
            return b.ToString();
        }
    }
}
