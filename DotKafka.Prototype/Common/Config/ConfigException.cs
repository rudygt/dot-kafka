using System;
using DotKafka.Prototype.Common.Errors;

namespace DotKafka.Prototype.Common.Config
{
    public class ConfigException : KafkaException
    {

        public ConfigException(string message) : base(message) { }

        public ConfigException(string name, object value) : this(name, value, null) { }

        public ConfigException(string name, object value, string message, Exception e) : base("Invalid value " + value + " for configuration " + name + (message == null ? "" : ": " + message), e) { }

        public ConfigException(string name, object value, string message) : base("Invalid value " + value + " for configuration " + name + (message == null ? "" : ": " + message)) { }
    }
}
