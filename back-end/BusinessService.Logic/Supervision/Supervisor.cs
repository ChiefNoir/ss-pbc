using System;
using System.Threading.Tasks;

namespace BusinessService.Logic.Supervision
{
    /// <summary> Utility class that provides wrapper for all things that might fail </summary>
    /// <remarks> Service <b>CAN NOT</b> throw <seealso cref="Exception"/> and die</remarks>
    public static class Supervisor
    {
        /// <summary> Execute function and return its result if everything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="func"><seealso cref="Func"/> to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        public static async Task<ExecutionResult<T>> SafeExecuteAsync<T>(Func<Task<T>> func)
        {
            var result = new ExecutionResult<T>();

            try
            {
                result.Data = await func();
                result.IsSucceed = true;
            }
            catch (Exception ee)
            {
                //TODO: log error
                result.IsSucceed = false;
                result.ErrorMessage = $"{ee.Message}{Environment.NewLine} {ee.InnerException?.Message}";
            }

            return result;
        }

    }
}