using API.Helpers;
using GeneralTests.SharedUtils;
using System;
using Xunit;

namespace GeneralTests.API.HelpersTests
{
    public class Utils_Tests
    {

        [Theory]
        [InlineData("cute.jpg", "https://localhost:44386/files/cute.jpg")]
        [InlineData("cute.png", "https://localhost:44386/files/cute.png")]
        [InlineData("cute.doc", "https://localhost:44386/files/cute.doc")]
        [InlineData("cute.xlst", "https://localhost:44386/files/cute.xlst")]
        [InlineData("cute.exe", "https://localhost:44386/files/cute.exe")]
        internal void AppendUrlToName_Valid(string name, string expected)
        {
            var result = Utils.AppendUrlToName(Storage.CreateConfiguration(), name);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        internal void AppendUrlToName_Invalid(string name)
        {
            Assert.ThrowsAny<Exception>
            (
                () => Utils.AppendUrlToName(Storage.CreateConfiguration(), name)
            );
        }


    }
}
