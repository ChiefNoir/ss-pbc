using Infrastructure.Helpers;
using Xunit;

namespace GeneralTests.InfrastructureTests.HelpersTests
{
    public class Sanitizer_Tests
    {
        [Theory]
        [InlineData("12", "12")]
        [InlineData("SoMeCoDe", "somecode")]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("\\//", "")]
        public void SanitizeCode_Valid(string input, string expected)
        {
            var result = Sanitizer.SanitizeCode(input);
            Assert.Equal(result, expected);
        }

    }
}
