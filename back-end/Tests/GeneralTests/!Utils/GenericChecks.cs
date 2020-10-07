using Abstractions.Supervision;
using Xunit;

namespace GeneralTests._Utils
{
    internal static class GenericChecks
    {
        public static void CheckValid<T>(ExecutionResult<T> result)
        {
            Assert.NotEqual(default, result.Data);
            Assert.True(result.IsSucceed);
            Assert.Null(result.Error);
        }

        public static void CheckInvalid<T>(ExecutionResult<T> result)
        {
            Assert.Equal(default, result.Data);
            Assert.False(result.IsSucceed);
            Assert.NotNull(result.Error);
        }
    }
}