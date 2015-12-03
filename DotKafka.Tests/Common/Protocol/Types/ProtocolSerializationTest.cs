using DotKafka.Prototype.Common.Protocol.Types;
using FluentAssertions;
using System.IO;
using System.Text;
using Xunit;

namespace DotKafka.Tests.Common.Protocol.Types
{
    public class ProtocolSerializationTest
    {
        private Schema schema;
        private Struct s;

        public ProtocolSerializationTest()
        {
            this.schema = new Schema(new Field("int8", KafkaTypesHelper.Int8),
                             new Field("int16", KafkaTypesHelper.Int16),
                             new Field("int32", KafkaTypesHelper.Int32),
                             new Field("int64", KafkaTypesHelper.Int64),
                             new Field("string", KafkaTypesHelper.String),
                             new Field("bytes", KafkaTypesHelper.Bytes),
                             new Field("array", new KafkaArrayOf(KafkaTypesHelper.Int32)),
                             new Field("struct", new Schema(new Field("field", KafkaTypesHelper.Int32))));

            this.s = new Struct(this.schema).Set("int8", (byte)1)
                                             .Set("int16", (short)1)
                                             .Set("int32", 1)
                                             .Set("int64", 1L)
                                             .Set("string", "1")
                                             .Set("bytes", Encoding.UTF8.GetBytes("1"))
                                             .Set("array", new object[] { 1 });

            this.s.Set("struct", this.s.Instance("struct").Set("field", new object[] { 1, 2, 3 }));
        }

        [Fact]
        public void TestSimple()
        {
            check(KafkaTypesHelper.Int8, (sbyte)-111);
            check(KafkaTypesHelper.Int16, (short)-11111);
            check(KafkaTypesHelper.Int32, -11111111);
            check(KafkaTypesHelper.Int64, -11111111111L);
            check(KafkaTypesHelper.String, "");
            check(KafkaTypesHelper.String, "hello");
            check(KafkaTypesHelper.String, "A\u00ea\u00f1\u00fcC");
            check(KafkaTypesHelper.Bytes, new MemoryStream(new byte[0]));
            check(KafkaTypesHelper.Bytes, new MemoryStream(Encoding.UTF8.GetBytes("abcd")));
            check(new KafkaArrayOf(KafkaTypesHelper.Int32), new object[] { 1, 2, 3, 4 });
            check(new KafkaArrayOf(KafkaTypesHelper.String), new object[] { });
            check(new KafkaArrayOf(KafkaTypesHelper.String), new object[] { "hello", "there", "beautiful" });
        }

        private object RoundTrip(IType type, object obj)
        {
            var buffer = new MemoryStream(new byte[type.SizeOf(obj)]);
            type.Write(buffer, obj);
            //assertFalse("The buffer should now be full.", buffer.hasRemaining());
            buffer.Position.Should().Be(buffer.Length);            

            buffer.Position = 0;

            object read = type.Read(buffer);

            //assertFalse("All bytes should have been read.", buffer.hasRemaining());
            buffer.Position.Should().Be(buffer.Length);

            return read;
        }

        private void check(IType type, object obj)
        {
            object result = RoundTrip(type, obj);
            if (obj is object[]) {
                obj =(object[])obj;
                result =(object[])result;
            }
            //assertEquals("The object read back should be the same as what was written.", obj, result);
            if( obj is MemoryStream )
            {
                var a = (MemoryStream)obj;
                var b = (MemoryStream)result;
                a.ToArray().Should().BeEquivalentTo(b.ToArray());
            } else
                obj.ShouldBeEquivalentTo(result);
        }
    }
}
