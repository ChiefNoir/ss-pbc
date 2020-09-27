using Abstractions.IRepository;
using Abstractions.Supervision;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Security.Tests
{
    public class SupervisorTests
    {
        private readonly ISupervisor _supervisor;
        private readonly Mock<IConfiguration> _config = new Mock<IConfiguration>();
        private readonly Mock<ILogRepository> _log = new Mock<ILogRepository>();

        public SupervisorTests()
        {
            _supervisor = new Supervisor(_config.Object, _log.Object);
        }


        [Fact]
        public void SafeExecute_Valid()
        {
            var resultString = _supervisor.SafeExecute(() => { return "text"; } );
            Assert.True(resultString.Data == "text");
            Assert.True(resultString.IsSucceed);
            Assert.True(resultString.Error == null);

            var resultInt = _supervisor.SafeExecute(() => { return 12; });
            Assert.True(resultInt.Data == 12);
            Assert.True(resultInt.IsSucceed);
            Assert.True(resultInt.Error == null);

            var resultCollection = _supervisor.SafeExecute(() => { return new List<int> { 42 } ; });
            Assert.True(resultCollection.Data.Count == 1);
            Assert.True(resultCollection.Data[0] == 42);
            Assert.True(resultCollection.IsSucceed);
            Assert.True(resultCollection.Error == null);
        }


        [Fact]
        public void SafeExecute_Invalid()
        {
            var resultString = _supervisor.SafeExecute<string>(() => { throw new Exception("One"); });
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);
            Assert.True(resultString.Error.Message == "One");
            Assert.True(resultString.Error.Detail == null);

            var resultInt = _supervisor.SafeExecute<int>(() => { throw new Exception("Two"); });
            Assert.True(resultInt.Data == default);
            Assert.True(resultInt.IsSucceed == false);
            Assert.True(resultInt.Error != null);
            Assert.True(resultInt.Error.Message == "Two");
            Assert.True(resultInt.Error.Detail == null);

            var resultCollection = _supervisor.SafeExecute<List<int>>(() => { throw new Exception("Three"); });
            Assert.True(resultCollection.Data == default);
            Assert.True(resultCollection.IsSucceed == false);
            Assert.True(resultCollection.Error != null);
            Assert.True(resultCollection.Error.Message == "Three");
            Assert.True(resultCollection.Error.Detail == null);


            var result = _supervisor.SafeExecute<int>(() => { throw new Exception("Main", new Exception("Inner")); });
            Assert.True(result.Data == default);
            Assert.True(result.IsSucceed == false);
            Assert.True(result.Error != null);
            Assert.True(result.Error.Message == "Main");
            Assert.True(result.Error.Detail == "Inner");
        }


        [Fact]
        public async void SafeExecuteAsync_Valid()
        {
            var resultString = await _supervisor.SafeExecuteAsync(() => { return Task.FromResult("text"); });
            Assert.True(resultString.Data == "text");
            Assert.True(resultString.IsSucceed);
            Assert.True(resultString.Error == null);

            var resultInt = await _supervisor.SafeExecuteAsync(() => { return Task.FromResult(12); });
            Assert.True(resultInt.Data == 12);
            Assert.True(resultInt.IsSucceed);
            Assert.True(resultInt.Error == null);

            var resultCollection  = await _supervisor.SafeExecuteAsync(() => { return Task.FromResult(new List<int> { 42 }); });
            Assert.True(resultCollection.Data.Count == 1);
            Assert.True(resultCollection.Data[0] == 42);
            Assert.True(resultCollection.IsSucceed);
            Assert.True(resultCollection.Error == null);
        }


        [Fact]
        public async void SafeExecuteAsync_Invalid()
        {
            var resultString = await _supervisor.SafeExecuteAsync<string>(() => { throw new Exception("One"); });
            Assert.True(resultString.Data == default);
            Assert.True(resultString.IsSucceed == false);
            Assert.True(resultString.Error != null);
            Assert.True(resultString.Error.Message == "One");
            Assert.True(resultString.Error.Detail == null);

            var resultInt = await _supervisor.SafeExecuteAsync<int>(() => { throw new Exception("Two"); });
            Assert.True(resultInt.Data == default);
            Assert.True(resultInt.IsSucceed == false);
            Assert.True(resultInt.Error != null);
            Assert.True(resultInt.Error.Message == "Two");
            Assert.True(resultInt.Error.Detail == null);

            var resultCollection = await _supervisor.SafeExecuteAsync<List<int>>(() => { throw new Exception("Three"); });
            Assert.True(resultCollection.Data == default);
            Assert.True(resultCollection.IsSucceed == false);
            Assert.True(resultCollection.Error != null);
            Assert.True(resultCollection.Error.Message == "Three");
            Assert.True(resultCollection.Error.Detail == null);


            var result = await _supervisor.SafeExecuteAsync<int>(() => { throw new Exception("Main", new Exception("Inner")); });
            Assert.True(result.Data == default);
            Assert.True(result.IsSucceed == false);
            Assert.True(result.Error != null);
            Assert.True(result.Error.Message == "Main");
            Assert.True(result.Error.Detail == "Inner");
        }

    }
}
