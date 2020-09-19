using System;
using System.Threading.Tasks;

namespace Abstractions.Supervision
{
    /// <summary> Utility class that provides wrapper for all things that might fail </summary>
    /// <remarks> Service <b>CAN NOT</b> throw <seealso cref="Exception"/> and die</remarks>
    public interface ISupervisor
    {
        /// <summary> Execute function and return its result if everything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="func"><seealso cref="Func"/> to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        Task<ExecutionResult<T>> SafeExecuteAsync<T>(Func<Task<T>> func);

        /// <summary> Execute function and return its result if everything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="func"><seealso cref="Func"/> to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        ExecutionResult<T> SafeExecute<T>(Func<T> func);

        /// <summary> Execute function and return its result if everything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="token">JWT token to validate</param>
        /// <param name="roles">Roles who have rights to execute function</param>
        /// <param name="func">Function to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        Task<ExecutionResult<T>> SafeExecuteAsync<T>(string token, string[] roles, Func<Task<T>> func);

        /// <summary> Execute function and return its result if everything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="token">JWT token to validate</param>
        /// <param name="roles">Roles who have rights to execute function</param>
        /// <param name="func">Function to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        ExecutionResult<T> SafeExecute<T>(string token, string[] roles, Func<T> func);
    }
}
