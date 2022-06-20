using Security;

namespace GeneralTests.Security
{
    public sealed class Supervisor_Tests
    {
        private readonly Supervisor _supervisor = new Supervisor();

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        public void SafeExecute_Valid(int value)
        {
            var result = _supervisor.SafeExecute(() => { return value; });

            Validator.CheckSucceed(result);
            Assert.Equal(value, result.Data);
        }

        [Fact]
        public void SafeExecute_Invalid()
        {
            var resultString = _supervisor.SafeExecute<string>(() => throw new Exception("one"));

            Validator.CheckFail(resultString);
            Assert.Equal("one", resultString.Error.Message);
            Assert.Null(resultString.Error.Detail);


            var resultInt = _supervisor.SafeExecute<int>(() => throw new Exception("one", new Exception("two")));

            Validator.CheckFail(resultInt);
            Assert.Equal("one", resultInt.Error.Message);
            Assert.Equal("two", resultInt.Error.Detail);
        }

        [Theory]
        [InlineData("text")]
        [InlineData("")]
        public async Task SafeExecuteAsync_Valid(string value)
        {
            var result = await _supervisor.SafeExecuteAsync(() => Task.FromResult(value));

            Validator.CheckSucceed(result);
            Assert.Equal(value, result.Data);
        }

        [Fact]
        public async Task SafeExecuteAsync_Invalid()
        {
            var resultString = await _supervisor.SafeExecuteAsync<string>(() => throw new Exception("One"));

            Validator.CheckFail(resultString);
            Assert.Equal("One", resultString.Error.Message);
            Assert.Null(resultString.Error.Detail);

            var result = await _supervisor.SafeExecuteAsync<int>(() => throw new Exception("Main", new Exception("Inner")));
            Validator.CheckFail(result);

            Assert.Equal("Main", result.Error.Message);
            Assert.Equal("Inner", result.Error.Detail);
        }

    }
}
