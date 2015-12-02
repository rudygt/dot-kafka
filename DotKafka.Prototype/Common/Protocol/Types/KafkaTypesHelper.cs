namespace DotKafka.Prototype.Common.Protocol.Types
{
    public class KafkaTypesHelper
    {
        public static IType Int8 = new KafkaInt8();
        public static IType Int16 = new KafkaInt16();
        public static IType Int32 = new KafkaInt32();
        public static IType Int64 = new KafkaInt64();
        public static IType String = new KafkaString();
        public static IType Bytes = new KafkaBytes();
    }
}