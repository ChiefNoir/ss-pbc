using System;
using Xunit;
using Common.Helpers;

namespace GeneralTests.Common.HelpersTests
{
    public class Helpers_Tests
    {
        [Theory]
        [InlineData("asdf.txt", "asdf.txt")]
        [InlineData("\"<>|:*?\\/.txt", "_________.txt")]
        [InlineData("yes_its_valid_~!@#$%^&()_+.txt", "yes_its_valid_~!@#$%^&()_+.txt")]
        public void SanitizedFileName_Valid(string fileName, string expected)
        {
            // https://msdn.microsoft.com/en-us/library/aa365247.aspx#naming_conventions
            var actual = Helpers.SanitizedFileName(fileName);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("asdf.txt", "asdf.txt")]
        [InlineData("\"<>|:*?\\/.txt", "repreprepreprepreprepreprep.txt")]
        [InlineData("yes_its_valid_~!@#$%^&()_+.txt", "yes_its_valid_~!@#$%^&()_+.txt")]
        public void SanitizedFileName_Valid_Replace(string fileName, string expected)
        {
            // https://msdn.microsoft.com/en-us/library/aa365247.aspx#naming_conventions
            var actual = Helpers.SanitizedFileName(fileName, "rep");

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SanitizedFileName_Inalid(string fileName)
        {
            Assert.ThrowsAny<Exception>(() => Helpers.SanitizedFileName(fileName));
        }
    }
}
