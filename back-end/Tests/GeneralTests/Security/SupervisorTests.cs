using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Supervision;
using GeneralTests.SharedMocks;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace GeneralTests.Security
{
    public class SupervisorTests
    {
        private readonly ISupervisor _supervisor;
        private readonly Mock<ITokenManager> _tokenMock = new Mock<ITokenManager>();

        public SupervisorTests()
        {
            _tokenMock.Setup(x => x.ValidateToken("valid")).Returns(new MockPrincipal(new ClaimsIdentity
                    (
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Role, "valid")
                        }
                    )));
            _tokenMock.Setup(x => x.ValidateToken("invalid")).Returns(new MockPrincipal(new ClaimsIdentity()));
            _tokenMock.Setup(x => x.ValidateToken("SecurityTokenException")).Returns(() => throw new SecurityTokenException());
            _tokenMock.Setup(x => x.ValidateToken("IPrincipal-null")).Returns<IPrincipal>(null);

            _supervisor = new Supervisor(_tokenMock.Object);
        }


        [Theory]
        [InlineData(12)]
        public void SafeExecute_Valid(int value)
        {
            var resultString = _supervisor.SafeExecute(() => value);

            Assert.Equal(value, resultString.Data);
            Assert.True(resultString.IsSucceed);
            Assert.Null(resultString.Error);
        }

        [Fact]
        public void SafeExecute_Invalid()
        {
            var resultString = _supervisor.SafeExecute<string>(() => throw new Exception("one"));
            Assert.Null(resultString.Data);
            Assert.False(resultString.IsSucceed);
            Assert.NotNull(resultString.Error);

            Assert.Equal("one", resultString.Error.Message);
            Assert.Null(resultString.Error.Detail);


            var resultInt = _supervisor.SafeExecute<int>(() => throw new Exception("one", new Exception("two")));
            Assert.Equal(default, resultInt.Data);
            Assert.False(resultInt.IsSucceed);
            Assert.NotNull(resultInt.Error);

            Assert.Equal("one", resultInt.Error.Message);
            Assert.Equal("two", resultInt.Error.Detail);
        }



        [Fact]
        public async void SafeExecuteAsync_Valid()
        {
            var resultString = await _supervisor.SafeExecuteAsync(() => Task.FromResult("text"));

            Assert.NotNull(resultString.Data);
            Assert.Equal("text", resultString.Data);
            Assert.True(resultString.IsSucceed);
            Assert.Null(resultString.Error);
        }

        [Fact]
        public async void SafeExecuteAsync_Invalid()
        {
            var resultString = await _supervisor.SafeExecuteAsync<string>(() => throw new Exception("One"));
            Assert.Null(resultString.Data);
            Assert.False(resultString.IsSucceed);
            Assert.NotNull(resultString.Error);

            Assert.Equal("One", resultString.Error.Message);
            Assert.Null(resultString.Error.Detail);

            var result = await _supervisor.SafeExecuteAsync<int>(() => throw new Exception("Main", new Exception("Inner")));
            Assert.Equal(default, result.Data);
            Assert.False(result.IsSucceed);
            Assert.NotNull(result.Error);

            Assert.Equal("Main", result.Error.Message);
            Assert.Equal("Inner", result.Error.Detail);
        }




        [Fact]
        public void SafeExecuteWithToken_Valid()
        {
            var resultString = _supervisor.SafeExecute("valid", new[] { "valid" }, () => "text");

            Assert.NotNull(resultString.Data);
            Assert.Equal("text", resultString.Data);
            Assert.True(resultString.IsSucceed);
            Assert.Null(resultString.Error);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData(null)]
        [InlineData("SecurityTokenException")]
        [InlineData("IPrincipal-null")]
        public void SafeExecuteWithToken_InValid(string token)
        {
            var resultString = _supervisor.SafeExecute(token, new[] { "invalid" }, () => "text");
            Assert.Null(resultString.Data);
            Assert.False(resultString.IsSucceed);
            Assert.NotNull(resultString.Error);
        }


        [Fact]
        public async void SafeExecuteAsyncWithToken_Valid()
        {
            var resultString = await _supervisor.SafeExecuteAsync("valid", new[] { "valid" }, () => Task.FromResult("text"));

            Assert.NotNull(resultString.Data);
            Assert.Equal("text", resultString.Data);
            Assert.True(resultString.IsSucceed);
            Assert.Null(resultString.Error);
        }

        [Theory]
        [InlineData("")]
        [InlineData("invalid")]
        [InlineData(null)]
        [InlineData("SecurityTokenException")]
        [InlineData("IPrincipal-null")]
        public async void SafeExecuteAsyncWithToken_InValid(string token)
        {
            var resultString = await _supervisor.SafeExecuteAsync(token, new[] { "invalid" }, () => Task.FromResult("text"));

            Assert.Null(resultString.Data);
            Assert.False(resultString.IsSucceed);
            Assert.NotNull(resultString.Error);
        }

    }
}
