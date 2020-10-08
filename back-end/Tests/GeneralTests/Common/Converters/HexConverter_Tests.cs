using Common.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeneralTests.Common.ConvertersTests
{
    public class HexConverter_Tests
    {
        class ValidPlainText : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    "plainText",
                    "706c61696e54657874"
                };
                yield return new object[]
                {
                    "706c61696e54657874",
                    "373036633631363936653534363537383734"
                };
                yield return new object[]
                {
                    "long plain text",
                    "6c6f6e6720706c61696e2074657874"
                };
                yield return new object[]
                {
                    string.Empty,
                    string.Empty
                };
                yield return new object[]
                {
                    null,
                    string.Empty
                };
            }
        }

        [Theory]
        [ClassData(typeof(ValidPlainText))]
        public void ToHexString_Valid(string plainText, string expectedHex)
        {
            var utfBytes = plainText == null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(plainText);
            var result = HexConverter.ToHexString(utfBytes);

            Assert.Equal(expectedHex, result);
        }

        [Theory]
        [ClassData(typeof(ValidPlainText))]
        public void ToByteArray_Valid(string expectedPlainText, string hex)
        {
            var result = HexConverter.ToByteArray(hex);

            var expectedArray = expectedPlainText == null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(expectedPlainText);
            
            Assert.Equal(expectedArray, result);
        }

    }
}
