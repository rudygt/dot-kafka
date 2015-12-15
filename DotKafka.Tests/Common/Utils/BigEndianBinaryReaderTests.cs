using DotKafka.Prototype.Common.Utils;
using System;
using System.IO;
using Xunit;

namespace DotKafka.Tests.Common.Utils
{
    public class BigEndianBinaryReaderTests
    {        

        // validates my assumptions about the default implementation doing the opposite of this implementation
        [Theory]
        [InlineData((Int32)0, new Byte[] { 0x00, 0x00, 0x00, 0x00 })]
        [InlineData((Int32)1, new Byte[] { 0x01, 0x00, 0x00, 0x00 })]
        [InlineData((Int32)(-1), new Byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
        [InlineData(Int32.MinValue, new Byte[] { 0x00, 0x00, 0x00, 0x80 })]
        [InlineData(Int32.MaxValue, new Byte[] { 0xFF, 0xFF, 0xFF, 0x7F })]
        public void NativeBinaryWriterTests(Int32 expectedValue, Byte[] givenBytes)
        {
            // arrange
            var memoryStream = new MemoryStream(givenBytes);
            var binaryReader = new BinaryReader(memoryStream);

            // act
            var actualValue = binaryReader.ReadInt32();

            // assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData((Int32)0, new Byte[] { 0x00, 0x00, 0x00, 0x00 })]
        [InlineData((Int32)1, new Byte[] { 0x00, 0x00, 0x00, 0x01 })]
        [InlineData((Int32)(-1), new Byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
        [InlineData(Int32.MinValue, new Byte[] { 0x80, 0x00, 0x00, 0x00 })]
        [InlineData(Int32.MaxValue, new Byte[] { 0x7F, 0xFF, 0xFF, 0xFF })]
        public void Int32Tests(Int32 expectedValue, Byte[] givenBytes)
        {
            // arrange
            var memoryStream = new MemoryStream(givenBytes);
            var binaryReader = new BigEndianBinaryReader(memoryStream);

            // act
            var actualValue = binaryReader.ReadInt32();

            // assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData((UInt32)0, new Byte[] { 0x00, 0x00, 0x00, 0x00 })]
        [InlineData((UInt32)1, new Byte[] { 0x00, 0x00, 0x00, 0x01 })]
        [InlineData((UInt32)123456789, new Byte[] { 0x07, 0x5B, 0xCD, 0x15 })]
        [InlineData(UInt32.MinValue, new Byte[] { 0x00, 0x00, 0x00, 0x00 })]
        [InlineData(UInt32.MaxValue, new Byte[] { 0xFF, 0xFF, 0xFF, 0xFF })]
        public void UInt32Tests(UInt32 expectedValue, Byte[] givenBytes)
        {
            // arrange
            var memoryStream = new MemoryStream(givenBytes);
            var binaryReader = new BigEndianBinaryReader(memoryStream);

            // act
            var actualValue = binaryReader.ReadUInt32();

            // assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData((Single)(0), new Byte[] { 0x00, 0x00, 0x00, 0x00 })]
        [InlineData((Single)(1), new Byte[] { 0x3F, 0x80, 0x00, 0x00 })]
        [InlineData((Single)(-1), new Byte[] { 0xBF, 0x80, 0x00, 0x00 })]
        [InlineData(Single.MinValue, new Byte[] { 0xFF, 0x7F, 0xFF, 0xFF })]
        [InlineData(Single.MaxValue, new Byte[] { 0x7F, 0x7F, 0xFF, 0xFF })]
        [InlineData(Single.PositiveInfinity, new Byte[] { 0x7F, 0x80, 0x00, 0x00 })]
        [InlineData(Single.NegativeInfinity, new Byte[] { 0xFF, 0x80, 0x00, 0x00 })]
        [InlineData(Single.NaN, new Byte[] { 0xFF, 0xC0, 0x00, 0x00 })]
        public void SingleTests(Single expectedValue, Byte[] givenBytes)
        {
            // arrange
            var memoryStream = new MemoryStream(givenBytes);
            var binaryReader = new BigEndianBinaryReader(memoryStream);

            // act
            var actualValue = binaryReader.ReadSingle();

            // assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData((Double)(0), new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
        [InlineData((Double)(1), new Byte[] { 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
        [InlineData((Double)(-1), new Byte[] { 0xBF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
        [InlineData(Double.MinValue, new Byte[] { 0xFF, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
        [InlineData(Double.MaxValue, new Byte[] { 0x7F, 0xEF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF })]
        [InlineData(Double.PositiveInfinity, new Byte[] { 0x7F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
        [InlineData(Double.NegativeInfinity, new Byte[] { 0xFF, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
        [InlineData(Double.NaN, new Byte[] { 0xFF, 0xF8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 })]
        public void DoubleTests(Double expectedValue, Byte[] givenBytes)
        {
            // arrange
            var memoryStream = new MemoryStream(givenBytes);
            var binaryReader = new BigEndianBinaryReader(memoryStream);

            // act
            var actualValue = binaryReader.ReadDouble();

            // assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData("0000", new Byte[] { 0x04, 0x30, 0x30, 0x30, 0x30 })]
        [InlineData("€€€€", new Byte[] { 0x0C, 0xE2, 0x82, 0xAC, 0xE2, 0x82, 0xAC, 0xE2, 0x82, 0xAC, 0xE2, 0x82, 0xAC })]
        public void StringTests(String expectedValue, Byte[] givenBytes)
        {
            // arrange
            var memoryStream = new MemoryStream(givenBytes);
            var binaryReader = new BigEndianBinaryReader(memoryStream);

            // act
            var actualValue = binaryReader.ReadString();

            // assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData('0', new Byte[] { 0x30 })]
        [InlineData('€', new Byte[] { 0xE2, 0x82, 0xAC })]
        public void CharTests(Char expectedValue, Byte[] givenBytes)
        {
            // arrange
            var memoryStream = new MemoryStream(givenBytes);
            var binaryReader = new BigEndianBinaryReader(memoryStream);

            // act
            var actualValue = binaryReader.ReadChar();

            // assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(new Char[] { '0', '0', '0', '0' }, new Byte[] { 0x30, 0x30, 0x30, 0x30 })]
        [InlineData(new Char[] { '€', '€', '€', '€' }, new Byte[] { 0xE2, 0x82, 0xAC, 0xE2, 0x82, 0xAC, 0xE2, 0x82, 0xAC, 0xE2, 0x82, 0xAC })]
        public void CharArrayTests(Char[] expectedValue, Byte[] givenBytes)
        {
            // arrange
            var memoryStream = new MemoryStream(givenBytes);
            var binaryReader = new BigEndianBinaryReader(memoryStream);

            // act
            var actualValue = binaryReader.ReadChars(givenBytes.Length);

            // assert
            Assert.Equal(expectedValue, actualValue);
        }

    }
}
