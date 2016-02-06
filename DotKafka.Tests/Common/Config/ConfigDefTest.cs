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
            Assert.Equal(new List<string> { "a", "b", "c" }, vals["d"]);
            Assert.Equal(42.5d, vals["e"]);
            //Assert.Equal(String.class, vals.get("f"));
            Assert.Equal(true, vals["g"]);
            Assert.Equal(false, vals["h"]);
            Assert.Equal(true, vals["i"]);
            Assert.Equal(new Password("password"), vals["j"]);
            Assert.Equal(Password.Hidden, vals["j"].ToString());

        }
}
}
