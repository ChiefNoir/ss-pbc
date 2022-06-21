using SSPBC.Helpers;

namespace GeneralTests.SSPBC.Helpers
{
    [Trait("Category", "Unit")]
    public sealed class Utils_Tests
    {
        [Theory]
        [InlineData("cute.jpg", "http://localhost:5000/static/cute.jpg")]
        [InlineData("cute.png", "http://localhost:5000/static/cute.png")]
        [InlineData("cute.doc", "http://localhost:5000/static/cute.doc")]
        [InlineData("cute.xlst", "http://localhost:5000/static/cute.xlst")]
        [InlineData("cute.exe", "http://localhost:5000/static/cute.exe")]
        internal void AppendUrlToName_Valid(string name, string expected)
        {
            var result = Utils.AppendUrlToName(Initializer.CreateConfiguration(), name);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        internal void AppendUrlToName_Invalid(string name)
        {
            Assert.ThrowsAny<Exception>
            (
                () => Utils.AppendUrlToName(Initializer.CreateConfiguration(), name)
            );
        }

    }
}
