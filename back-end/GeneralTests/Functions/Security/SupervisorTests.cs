using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Supervision;
using GeneralTests.Functions.SharedMocks;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace GeneralTests.Functions.Security
{
    public class SupervisorTests
    {
        private readonly ISupervisor _supervisor;
        private readonly Mock<ILogRepository> _log = new Mock<ILogRepository>();
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

            _supervisor = new Supervisor(_log.Object, _tokenMock.Object);
        }


        [Theory]
        [InlineData(12)]
        public void SafeExecute_Valid(int value)
        {
            var resultString = _supervisor.SafeExecute(() => value);
            Assert.True(resultString.Data == value);
            Assert.True(resultString.IsSucceed);
            Assert.True(resultString.Error == null);
        }

        [Fact]
        public void SafeExecute_Invalid()
        {
            var resultString = _supervisor.SafeExecute<string>(() => throw new Exception("one"));
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);
            Assert.True(resultString.Error.Message == "one");
            Assert.True(resultString.Error.Detail == null);


            var resultInt = _supervisor.SafeExecute<string>(() => throw new Exception("one", new Exception("two")));
            Assert.True(resultInt.Data == default);
            Assert.True(resultInt.IsSucceed == false);
            Assert.True(resultInt.Error != null);
            Assert.True(resultInt.Error.Message == "one");
            Assert.True(resultInt.Error.Detail == "two");
        }



        [Fact]
        public async void SafeExecuteAsync_Valid()
        {
            var resultString = await _supervisor.SafeExecuteAsync(() => Task.FromResult("text"));
            Assert.True(resultString.Data == "text");
            Assert.True(resultString.IsSucceed);
            Assert.True(resultString.Error == null);
        }

        [Fact]
        public async void SafeExecuteAsync_Invalid()
        {
            var resultString = await _supervisor.SafeExecuteAsync<string>(() => throw new Exception("One"));
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);
            Assert.True(resultString.Error.Message == "One");
            Assert.True(resultString.Error.Detail == null);

            var result = await _supervisor.SafeExecuteAsync<int>(() => throw new Exception("Main", new Exception("Inner")));
            Assert.True(result.Data == default);
            Assert.True(result.IsSucceed == false);
            Assert.True(result.Error != null);
            Assert.True(result.Error.Message == "Main");
            Assert.True(result.Error.Detail == "Inner");
        }




        [Fact]
        public void SafeExecuteWithToken_Valid()
        {
            var resultString = _supervisor.SafeExecute("valid", new[] { "valid" }, () => "text");
            Assert.True(resultString.Data == "text");
            Assert.True(resultString.IsSucceed);
            Assert.True(resultString.Error == null);
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
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);
        }


        [Fact]
        public async void SafeExecuteAsyncWithToken_Valid()
        {
            var resultString = await _supervisor.SafeExecuteAsync("valid", new[] { "valid" }, () => Task.FromResult("text"));
            Assert.True(resultString.Data == "text");
            Assert.True(resultString.IsSucceed);
            Assert.True(resultString.Error == null);
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
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);
        }

    }
}
