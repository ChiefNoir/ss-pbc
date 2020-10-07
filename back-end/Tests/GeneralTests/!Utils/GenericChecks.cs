using Abstractions.Supervision;
using Xunit;

namespace GeneralTests._Utils
{
    internal static class GenericChecks
    {
        public static void CheckValid<T>(ExecutionResult<T> response)
        {
            Assert.NotEqual(default, response.Data);
            Assert.True(response.IsSucceed);
            Assert.Null(response.Error);
        }

        public static void CheckInvalid<T>(ExecutionResult<T> response)
        {
            Assert.Equal(default, response.Data);
            Assert.False(response.IsSucceed);
            Assert.NotNull(response.Error);
        }
    }
}