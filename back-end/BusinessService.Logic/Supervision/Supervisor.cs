using System;

namespace BusinessService.Logic.Supervision
{
    /// <summary> Utility class that provides wrapper for all things that might fail </summary>
    /// <remarks> Service CAN NOT throw Exception and die</remarks>
    public static class Supervisor
    {
        /// <summary> Execute function and return its result if anything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="func">Function to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        public static ExecutionResult<T> SafeExecute<T>(Func<T> func)
        {
            var result = new ExecutionResult<T>();

            try
            {
                result.Data = func();
                result.IsSucceed = true;
            }
            catch (Exception ee)
            {
                result.IsSucceed = false;
                result.ErrorMessage = ee.Message;
            }

            return result;
        }

        /// <summary> Execute function and return empty result if anything goes right </summary>
        /// <param name="func">Function to execute</param>
        /// <returns>Empty result of execution or ErrorMessage will have message </returns>
        public static ExecutionResult<object> SafeExecute(Action func)
        {
            var result = new ExecutionResult<object>();

            try
            {
                func();
                result.IsSucceed = true;
            }
            catch (Exception ee)
            {
                result.IsSucceed = false;
                result.ErrorMessage = ee.Message;
            }

            return result;
        }
    }
}
