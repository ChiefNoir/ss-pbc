using System;

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

        /// <summary> Execute function </summary>
        /// <param name="action"><seealso cref="Action"/> to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        public static ExecutionResult SafeExecute(Action action)
        {
            var result = new ExecutionResult();

            try
            {
                action();
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
