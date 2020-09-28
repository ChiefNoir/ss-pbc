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


        [Fact]
        public void SafeExecute_Valid()
        {
            var resultString = _supervisor.SafeExecute(() => "text");
            Assert.True(resultString.Data == "text");
            Assert.True(resultString.IsSucceed);
            Assert.True(resultString.Error == null);

            var resultInt = _supervisor.SafeExecute(() => 12);
            Assert.True(resultInt.Data == 12);
            Assert.True(resultInt.IsSucceed);
            Assert.True(resultInt.Error == null);

            var resultCollection = _supervisor.SafeExecute(() => new List<int> { 42 });
            Assert.True(resultCollection.Data.Count == 1);
            Assert.True(resultCollection.Data[0] == 42);
            Assert.True(resultCollection.IsSucceed);
            Assert.True(resultCollection.Error == null);
        }


        [Fact]
        public void SafeExecute_Invalid()
        {
            var resultString = _supervisor.SafeExecute<string>(() => throw new Exception("One"));
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);
            Assert.True(resultString.Error.Message == "One");
            Assert.True(resultString.Error.Detail == null);

            var resultInt = _supervisor.SafeExecute<int>(() => throw new Exception("Two"));
            Assert.True(resultInt.Data == default);
            Assert.True(resultInt.IsSucceed == false);
            Assert.True(resultInt.Error != null);
            Assert.True(resultInt.Error.Message == "Two");
            Assert.True(resultInt.Error.Detail == null);

            var resultCollection = _supervisor.SafeExecute<List<int>>(() => throw new Exception("Three"));
            Assert.True(resultCollection.Data == default);
            Assert.True(resultCollection.IsSucceed == false);
            Assert.True(resultCollection.Error != null);
            Assert.True(resultCollection.Error.Message == "Three");
            Assert.True(resultCollection.Error.Detail == null);


            var result = _supervisor.SafeExecute<int>(() => throw new Exception("Main", new Exception("Inner")));
            Assert.True(result.Data == default);
            Assert.True(result.IsSucceed == false);
            Assert.True(result.Error != null);
            Assert.True(result.Error.Message == "Main");
            Assert.True(result.Error.Detail == "Inner");
        }


        [Fact]
        public async void SafeExecuteAsync_Valid()
        {
            var resultString = await _supervisor.SafeExecuteAsync(() => Task.FromResult("text"));
            Assert.True(resultString.Data == "text");
            Assert.True(resultString.IsSucceed);
            Assert.True(resultString.Error == null);

            var resultInt = await _supervisor.SafeExecuteAsync(() => Task.FromResult(12));
            Assert.True(resultInt.Data == 12);
            Assert.True(resultInt.IsSucceed);
            Assert.True(resultInt.Error == null);

            var resultCollection = await _supervisor.SafeExecuteAsync(() => Task.FromResult(new List<int> { 42 }));
            Assert.True(resultCollection.Data.Count == 1);
            Assert.True(resultCollection.Data[0] == 42);
            Assert.True(resultCollection.IsSucceed);
            Assert.True(resultCollection.Error == null);
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

            var resultInt = await _supervisor.SafeExecuteAsync<int>(() => throw new Exception("Two"));
            Assert.True(resultInt.Data == default);
            Assert.True(resultInt.IsSucceed == false);
            Assert.True(resultInt.Error != null);
            Assert.True(resultInt.Error.Message == "Two");
            Assert.True(resultInt.Error.Detail == null);

            var resultCollection = await _supervisor.SafeExecuteAsync<List<int>>(() => throw new Exception("Three"));
            Assert.True(resultCollection.Data == default);
            Assert.True(resultCollection.IsSucceed == false);
            Assert.True(resultCollection.Error != null);
            Assert.True(resultCollection.Error.Message == "Three");
            Assert.True(resultCollection.Error.Detail == null);


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

            var resultInt = _supervisor.SafeExecute("valid", null, () => 12);
            Assert.True(resultInt.Data == 12);
            Assert.True(resultInt.IsSucceed);
            Assert.True(resultInt.Error == null);

            var resultCollection = _supervisor.SafeExecute("valid", new[] { "valid" }, () => new List<int> { 42 });
            Assert.True(resultCollection.Data.Count == 1);
            Assert.True(resultCollection.Data[0] == 42);
            Assert.True(resultCollection.IsSucceed);
            Assert.True(resultCollection.Error == null);
        }

        [Fact]
        public void SafeExecuteWithToken_InValid()
        {
            var resultString = _supervisor.SafeExecute("invalid", new[] { "invalid" }, () => "text");
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);

            var resultInt = _supervisor.SafeExecute("invalid", new[] { "invalid" }, () => 12);
            Assert.True(resultInt.Data == default);
            Assert.True(resultInt.IsSucceed == false);
            Assert.True(resultInt.Error != null);

            var resultCollection = _supervisor.SafeExecute("invalid", new[] { "invalid" }, () => new List<int> { 42 });
            Assert.True(resultCollection.Data == default);

            Assert.True(resultCollection.IsSucceed == false);
            Assert.True(resultCollection.Error != null);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData(null)]
        [InlineData("SecurityTokenException")]
        [InlineData("IPrincipal-null")]
        public void SafeExecuteWithToken_InValidTokenVariations(string token)
        {
            var resultString = _supervisor.SafeExecute(token, new[] { "invalid" }, () => "text");
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);
        }









    }
}
