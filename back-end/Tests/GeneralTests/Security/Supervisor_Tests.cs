using Abstractions.ISecurity;
using Abstractions.Supervision;
using GeneralTests.SharedUtils;
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
using Microsoft.Extensions.Logging;

namespace GeneralTests.Security
{
    public class Supervisor_Tests
    {
        private readonly ISupervisor _supervisor;
        private readonly Mock<ITokenManager> _tokenMock = new Mock<ITokenManager>();
        private readonly Mock<ILogger<Supervisor>> _logger = new Mock<ILogger<Supervisor>>();

        public Supervisor_Tests()
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

            _supervisor = new Supervisor(_logger.Object);
        }


        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        public void SafeExecute_Valid(int value)
        {
            var result = _supervisor.SafeExecute(() => { return value; });
            
            GenericChecks.CheckSucceed(result);
            Assert.Equal(value, result.Data);
        }

        [Fact]
        public void SafeExecute_Invalid()
        {
            var resultString = _supervisor.SafeExecute<string>(() => throw new Exception("one"));
            
            GenericChecks.CheckFail(resultString);
            Assert.Equal("one", resultString.Error.Message);
            Assert.Null(resultString.Error.Detail);


            var resultInt = _supervisor.SafeExecute<int>(() => throw new Exception("one", new Exception("two")));

            GenericChecks.CheckFail(resultInt);
            Assert.Equal("one", resultInt.Error.Message);
            Assert.Equal("two", resultInt.Error.Detail);
        }

        [Theory]
        [InlineData("text")]
        [InlineData("")]
        public async void SafeExecuteAsync_Valid(string value)
        {
            var result = await _supervisor.SafeExecuteAsync(() => Task.FromResult(value));

            GenericChecks.CheckSucceed(result);
            Assert.Equal(value, result.Data);
        }

        [Fact]
        public async void SafeExecuteAsync_Invalid()
        {
            var resultString = await _supervisor.SafeExecuteAsync<string>(() => throw new Exception("One"));

            GenericChecks.CheckFail(resultString);
            Assert.Equal("One", resultString.Error.Message);
            Assert.Null(resultString.Error.Detail);

            var result = await _supervisor.SafeExecuteAsync<int>(() => throw new Exception("Main", new Exception("Inner")));
            GenericChecks.CheckFail(result);

            Assert.Equal("Main", result.Error.Message);
            Assert.Equal("Inner", result.Error.Detail);
        }

    }
}
