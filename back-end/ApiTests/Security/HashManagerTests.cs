using Security;
using System;
using Xunit;

namespace ApiTests.Security
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
        [InlineData
            (
                "sa", 
                "83ce1b2fafc2efaec0986e31311a064dad3a348f224aad85270289cbe57c4da15fda1f2b71f037520c1c79920767149ffdaadcfa49455fb53ffbb91312bf9d564bb4238e5e9b034b137c3518f4cfc63912daa2fca9c8d03e6a6187a9fdc595e07c91a45ae77ed17ee37dcafac475b9be3501828d9089febddd8cabad3b541992",
                "92cb4dc464f7008d722b7d9ab7fb7dbbec104b028b590c75bd9116b316e8250a581473c743cb4fd9bbc11760638e4495"
            )
        ]
        [InlineData
            (
                "sa",
                "d7365e8f9d27281b6c892b1d12be907c5432d9ac9ed0b85ff3c1d6eb8b6018d0f86f07e99f5845454f28afc947652b0bcb012fafba1ead672892af92be2487ba85de048845e4daad092f922509b6cd5df37161a0c2e656630beb395fc584e6f6bc2856b9a7fc43a415c0274c52ce1c907d354b735938e9f20bdab339f267cb57",
                "2a76eb8d7eca48ecd8ae53a820bea50925fda7cbfc8fab07ed94227332d0dfe7f8680b50d129fcf2c24ae1e71ef0fe26"
            )
        ]
        [InlineData
            (
                "sa",
                "a7b413afd8f99969bc79623ef2367c3226a446a125d433551fbd9ade843e4d390dba72305fd8f1720bda0c75d7dd9bf2f9df8ede812a810d9022399408a0dae4eba9c777e5d142a87c1348509fb696bbce94c81aa5cd5b2afa16d7b02498feedd228ca7b6274c7df2a72d0edec760ba1f89ee10d7077f5e0cd57f0f73c4f1332",
                "1fc01a726d6523f88e3c1757066bee8c9abd77bd6db6f286093d498456194c56ef695fd776db96e08bfaae380125dced"
            )
        ]
        public void Hash_HexHash_MustBeEqual(string plainText, string salt, string result)
        {
            var resultOne = _hashManager.Hash(plainText, salt);

            Assert.True(resultOne.HexHash.Equals(result));
            Assert.True(resultOne.HexSalt.Equals(salt));
        }
    }
}
