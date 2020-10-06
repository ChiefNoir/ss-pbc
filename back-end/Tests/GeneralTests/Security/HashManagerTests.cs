using Security;
using System;
using Xunit;

namespace GeneralTests.Security
{
    public class HashManagerTests
    {
        private readonly HashManager _hashManager;

        public HashManagerTests()
        {
            _hashManager = new HashManager();
        }

        [Theory]
        [InlineData("password", "password")]
        [InlineData("12", "12")]
        [InlineData(" ", " ")]
        public void Hash_HexHash_MustBeDifferent(string plainTextOne, string plainTextTwo)
        {
            var resultOne = _hashManager.Hash(plainTextOne);
            var resultTwo = _hashManager.Hash(plainTextTwo);

            Assert.False(resultOne.HexHash.Equals(resultTwo.HexHash));
            Assert.False(resultOne.HexSalt.Equals(resultTwo.HexSalt));
        }

        [Theory]
        [InlineData(null, "salt")]
        [InlineData("", "salt")]
        [InlineData("password", "")]
        public void Hash_HexHash_MustFail(string plainText, string salt)
        {
            Assert.Throws<ArgumentException>(() => _hashManager.Hash(plainText, salt));
        }


        [Theory]
        [InlineData("1")]
        [InlineData("sa")]
        [InlineData("qwerty")]
        [InlineData("EMfwzEp8jcx9sje8Fwwz")]
        [InlineData("df0af56a20f019f585d65de70c7614b75a179a180f542b102f5460b3ce8051ed")]
        public void Hash_HexHash_Restore(string plaintText)
        {
            var initialHashing = _hashManager.Hash(plaintText);

            var sameWithSalt = _hashManager.Hash(plaintText, initialHashing.HexSalt);

            Assert.Equal(initialHashing.HexHash, sameWithSalt.HexHash);
        }

    }
}
