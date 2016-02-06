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
    }
}
