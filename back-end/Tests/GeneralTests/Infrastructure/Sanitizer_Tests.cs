using Infrastructure.Helpers;

namespace GeneralTests.Infrastructure
{
    [Trait("Category", "Unit")]
    public sealed class Sanitizer_Tests
    {
        [Theory]
        [InlineData("12", "12")]
        [InlineData("SoMeCoDe", "somecode")]
        [InlineData("КрасныйSoMeCoDe", "somecode")]
        public void SanitizeCode_Valid(string input, string expected)
        {
            var result = Sanitizer.SanitizeCode(input);
            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("красный")]
        [InlineData("日本")]
        [InlineData("\\//")]
        public void SanitizeCode_Invalid(string input)
        {
            Assert.Throws<ArgumentNullException>(delegate { Sanitizer.SanitizeCode(input); });
        }
    }
}
