using Security;
using System.Data.Common;
using System.Runtime.Serialization;

namespace GeneralTests.Security
{
    [Trait("Category", "Unit")]
    public sealed class Supervisor_Tests
    {
        private readonly Supervisor _supervisor = new();

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
            Assert.Equal("one", resultString!.Error!.Message);
            Assert.Null(resultString.Error.Detail);


            var resultInt = _supervisor.SafeExecute<int>(() => throw new Exception("one", new Exception("two")));

            Validator.CheckFail(resultInt);
            Assert.Equal("one", resultInt!.Error!.Message);
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
            Assert.Equal("One", resultString!.Error!.Message);
            Assert.Null(resultString.Error.Detail);

            var result = await _supervisor.SafeExecuteAsync<int>(() => throw new Exception("Main", new Exception("Inner")));
            Validator.CheckFail(result);

            Assert.Equal("Main", result!.Error!.Message);
            Assert.Equal("Inner", result.Error.Detail);
        }

        [Fact]
        public async Task SafeExecuteAsync_MustHideDbException()
        {
            var resultString = await _supervisor.SafeExecuteAsync<string>(() => throw new TestDbDbException("Database"));

            Validator.CheckFail(resultString);
            Assert.NotEqual("Database", resultString!.Error!.Message);            
        }

        [Fact]
        public void SafeExecute_MustHideDbException()
        {
            var resultString = _supervisor.SafeExecute<string>(() => throw new TestDbDbException("Database"));

            Validator.CheckFail(resultString);
            Assert.NotEqual("Database", resultString!.Error!.Message);
        }
    }

    class TestDbDbException : DbException
    {
        public TestDbDbException() : base()
        {
        }

        public TestDbDbException(string message) : base(message)
        {
        }

        public TestDbDbException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TestDbDbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
