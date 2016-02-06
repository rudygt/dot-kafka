using System;
using System.Collections.Generic;
using DotKafka.Prototype.Common.Config;
using DotKafka.Prototype.Common.Config.Types;
using Xunit;

namespace DotKafka.Tests.Common.Config {
    public class ConfigDefTest {
        [Fact]
        public void TestBasicTypes() {
            var def = new ConfigDef().Define("a", ConfigDef.Type.Int, 5, ConfigDef.Range.Between(0, 14),
                ConfigDef.Importance.High, "docs")
                .Define("b", ConfigDef.Type.Long, ConfigDef.Importance.High, "docs")
                .Define("c", ConfigDef.Type.String, "hello", ConfigDef.Importance.High, "docs")
                .Define("d", ConfigDef.Type.List, ConfigDef.Importance.High, "docs")
                .Define("e", ConfigDef.Type.Double, ConfigDef.Importance.High, "docs")
                //.Define("f", ConfigDef.Type.Class, ConfigDef.Importance.High, "docs")
                .Define("g", ConfigDef.Type.Bool, ConfigDef.Importance.High, "docs")
                .Define("h", ConfigDef.Type.Bool, ConfigDef.Importance.High, "docs")
                .Define("i", ConfigDef.Type.Bool, ConfigDef.Importance.High, "docs")
                .Define("j", ConfigDef.Type.Password, ConfigDef.Importance.High, "docs");

            var props = new Dictionary<string, object>();
            props.Add("a", "1   ");
            props.Add("b", 2);
            props.Add("d", " a , b, c");
            props.Add("e", 42.5d);
            //props.Add("f", string.class.etName());
            props.Add("g", "true");
            props.Add("h", "FalSE");
            props.Add("i", "TRUE");
            props.Add("j", "password");

            var vals = def.Parse(props);

            Assert.Equal(1, vals["a"]);
            Assert.Equal(2L, vals["b"]);
            Assert.Equal("hello", vals["c"]);
            Assert.Equal(new List<string> {"a", "b", "c"}, vals["d"]);
            Assert.Equal(42.5d, vals["e"]);
            //Assert.Equal(String.class, vals.get("f"));
            Assert.Equal(true, vals["g"]);
            Assert.Equal(false, vals["h"]);
            Assert.Equal(true, vals["i"]);
            Assert.Equal(new Password("password"), vals["j"]);
            Assert.Equal(Password.Hidden, vals["j"].ToString());
        }

        [Fact]
        public void TestInvalidDefault() {
            try {
                new ConfigDef().Define("a", ConfigDef.Type.Int, "hello", ConfigDef.Importance.High, "docs");
            }
            catch (Exception x) {
                Assert.IsType(typeof (ConfigException), x);
            }
        }

        [Fact]
        public void TestNullDefault() {
            var def = new ConfigDef().Define("a", ConfigDef.Type.Int, null, null, ConfigDef.Importance.Medium, "docs");
            var vals = def.Parse(new Dictionary<string, object>());
            Assert.Equal(null, vals["a"]);
        }

        [Fact]
        public void TestMissingRequired() {
            try {
                new ConfigDef().Define("a", ConfigDef.Type.Int, ConfigDef.Importance.High, "docs").
                    Parse(new Dictionary<string, object>());
            }
            catch (Exception x) {
                Assert.IsType(typeof (ConfigException), x);
                Assert.Equal("Missing required configuration \"a\" which has no default value.", x.Message);
            }
        }

        [Fact]
        public void TestDefinedTwice() {
            try {
                new ConfigDef().Define("a", ConfigDef.Type.String, ConfigDef.Importance.High, "docs").
                    Define("a", ConfigDef.Type.Int, ConfigDef.Importance.High, "docs");
            }
            catch (Exception x) {
                Assert.IsType(typeof (ConfigException), x);
                Assert.Equal("Configuration a is defined twice.", x.Message);
            }
        }

        [Fact]
        public void TestBadInputs() {
            TestBadInputs(ConfigDef.Type.Int,
                new[] {"hello", "42.5", 42.5, long.MaxValue, Convert.ToString(long.MaxValue), new object()});
            TestBadInputs(ConfigDef.Type.Long,
                new[] {"hello", "42.5", Convert.ToString(long.MaxValue) + "00", new object()});
            TestBadInputs(ConfigDef.Type.Double,
                new[] {"hello", new object()});
            TestBadInputs(ConfigDef.Type.String,
                new[] {new object()});
            TestBadInputs(ConfigDef.Type.List,
                new[] {53, new object()});
            TestBadInputs(ConfigDef.Type.Bool,
                new[] {"hello", "truee", "fals"});
        }

        private void TestBadInputs(ConfigDef.Type type, IEnumerable<object> values) {
            foreach (var value in values) {
                var m = new Dictionary<string, object> {{"name", value}};
                var def = new ConfigDef().Define("name", type, ConfigDef.Importance.High, "docs");
                try {
                    def.Parse(m);
                    Assert.True(false, "Expected a config exception due to invalid value " + value);
                }
                catch (ConfigException x) {
                    //good to go
                }
            }
        }

        [Fact]
        public void TestInvalidDefaultRange() {
            try {
                new ConfigDef().Define("name", ConfigDef.Type.Int, -1, ConfigDef.Range.Between(0, 10),
                    ConfigDef.Importance.High, "docs");
            }
            catch (Exception x) {
                Assert.IsType(typeof (ConfigException), x);
                Assert.Equal("Invalid value -1 for configuration name: Value must be at least 0", x.Message);
            }
        }

        [Fact]
        public void TestInvalidDefaultString() {
            try {
                new ConfigDef().Define("name", ConfigDef.Type.String, "bad",
                    new ConfigDef.ValidString("valid", "values"), ConfigDef.Importance.High, "docs");
            }
            catch (Exception x) {
                Assert.IsType(typeof (ConfigException), x);
                Assert.Equal("Invalid value bad for configuration name: String must be one of: valid, values", x.Message);
            }
        }

        [Fact]
        public void TestValidators() {
            TestValidator(ConfigDef.Type.Int, ConfigDef.Range.Between(0, 10), 5, new object[] {1, 5, 9},
                new object[] {-1, 11});
            TestValidator(ConfigDef.Type.String, new ConfigDef.ValidString("good", "values", "default"), "default",
                new object[] {"good", "values", "default"}, new object[] {"bad", "inputs"});
        }

        private void TestValidator(ConfigDef.Type type, ConfigDef.IValidator validator, object defaultVal,
            object[] okValues, object[] badValues) {
            var def = new ConfigDef().Define("name", type, defaultVal, validator, ConfigDef.Importance.High, "docs");

            foreach (var value in okValues) {
                var m = new Dictionary<string, object> {{"name", value}};
                def.Parse(m);
            }

            foreach (var value in badValues) {
                var m = new Dictionary<string, object> {{"name", value}};
                try {
                    def.Parse(m);
                    Assert.True(false, "Expected a config exception due to invalid value " + value);
                }
                catch (ConfigException x) {
                    //good to go
                }
            }
        }

        [Fact]
        public void TestSslPasswords() {
            var def = new ConfigDef();
            SslConfigs.AddClientSslSupport(def);

            var props = new Dictionary<string, object> {
                {SslConfigs.SslKeyPasswordConfig, "key_password"},
                {SslConfigs.SslKeystorePasswordConfig, "keystore_password"},
                {SslConfigs.SslTruststorePasswordConfig, "truststore_password"}
            };

            var vals = def.Parse(props);

            Assert.Equal(new Password("key_password"), vals[SslConfigs.SslKeyPasswordConfig]);
            Assert.Equal(Password.Hidden, vals[SslConfigs.SslKeyPasswordConfig].ToString());
            Assert.Equal(new Password("keystore_password"), vals[SslConfigs.SslKeystorePasswordConfig]);
            Assert.Equal(Password.Hidden, vals[SslConfigs.SslKeystorePasswordConfig].ToString());
            Assert.Equal(new Password("truststore_password"), vals[SslConfigs.SslTruststorePasswordConfig]);
            Assert.Equal(Password.Hidden, vals[SslConfigs.SslTruststorePasswordConfig].ToString());
        }
    }
}
