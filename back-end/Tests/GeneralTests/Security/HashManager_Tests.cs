using Security;

namespace GeneralTests.Security
{
    [Trait("Category", "Unit")]
    public sealed class HashManager_Tests
    {
        [Theory]
        [InlineData("12")]
        [InlineData("test")]
        [InlineData("password")]
        public void Hash_Valid(string plainText)
        {
            var config = Initializer.CreateConfiguration();
            var hashManager = new HashManager(config);

            var result = hashManager.Hash(plainText);

            Assert.NotNull(result);
            Assert.NotNull(result.HexHash);
            Assert.NotNull(result.HexSalt);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Hash_Invalid(string plainText)
        {
            var config = Initializer.CreateConfiguration();
            var hashManager = new HashManager(config);

            Assert.Throws<ArgumentNullException>(() => hashManager.Hash(plainText));
        }

        [Theory]
        [InlineData("")]
        public void Hash_SaltInvalid(string salt)
        {
            var config = Initializer.CreateConfiguration();
            var hashManager = new HashManager(config);

            Assert.Throws<ArgumentNullException>(() => hashManager.Hash("text", salt));
        }


        [Theory]
        [InlineData("password")]
        [InlineData("12")]
        [InlineData(" ")]
        public void Hash_MustBeDifferent(string plainText)
        {
            var config = Initializer.CreateConfiguration();
            var hashManager = new HashManager(config);

            var resultOne = hashManager.Hash(plainText);
            var resultTwo = hashManager.Hash(plainText);

            Assert.NotEqual(resultTwo.HexHash, resultOne.HexHash);
            Assert.NotEqual(resultTwo.HexSalt, resultOne.HexSalt);
        }

        [Theory]
        [InlineData(null, "salt")]
        [InlineData("", "salt")]
        [InlineData("password", "")]
        public void Hash_MustFail(string plainText, string salt)
        {
            var config = Initializer.CreateConfiguration();
            var hashManager = new HashManager(config);

            Assert.Throws<ArgumentNullException>(() => hashManager.Hash(plainText, salt));
        }


        [Theory]
        [InlineData("1")]
        [InlineData("sa")]
        [InlineData("qwerty")]
        [InlineData("EMfwzEp8jcx9sje8Fwwz")]
        [InlineData("df0af56a20f019f585d65de70c7614b75a179a180f542b102f5460b3ce8051ed")]
        public void Hash_Restore(string plaintText)
        {
            var config = Initializer.CreateConfiguration();
            var hashManager = new HashManager(config);

            var initialHashing = hashManager.Hash(plaintText);

            var sameWithSalt = hashManager.Hash(plaintText, initialHashing.HexSalt);

            Assert.Equal(initialHashing.HexHash, sameWithSalt.HexHash);
        }

    }
}
