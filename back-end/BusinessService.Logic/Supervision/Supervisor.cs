using Abstractions.IFactory;
using Abstractions.Model.System;
using Microsoft.Extensions.Configuration;
using Security;
using System;
using System.Threading.Tasks;

namespace BusinessService.Logic.Supervision
{
    /// <summary> Utility class that provides wrapper for all things that might fail </summary>
    /// <remarks> Service <b>CAN NOT</b> throw <seealso cref="Exception"/> and die</remarks>
    public static class Supervisor
    {
        private static IConfiguration _configuration;

        public static void init(IConfiguration configuration)
        {
            _configuration = configuration;
        }


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
                result.Error = IncidentFactory.Create(IncidentsCodes.InternalError);
                result.Error.Detail = $"{ee.Message}{Environment.NewLine} {ee.InnerException?.Message}";
            }

            return result;
        }

        public static async Task<ExecutionResult<T>> SafeExecuteAsync<T>(string token, Func<Task<T>> func)
        {
            var result = new ExecutionResult<T>();

            try
            {
                var incident = CheckToken(token);

                if (incident != null)
                {
                    result.Error = incident;
                    return result;
                }

                result.Data = await func();
                result.IsSucceed = true;
            }
            catch (Exception ee)
            {
                //TODO: log error
                result.IsSucceed = false;
                result.Error = IncidentFactory.Create(IncidentsCodes.InternalError);
                result.Error.Detail = $"{ee.Message}{Environment.NewLine} {ee.InnerException?.Message}";
            }

            return result;
        }

        private static Incident CheckToken(string token, params string[] roles)
        {
            if (string.IsNullOrEmpty(token))
            {
                return IncidentFactory.Create(IncidentsCodes.AuthenticationNotProvided);
            }

            var principal = TokenManager.ValidateToken(_configuration, token);
            if (principal == null || principal.Identity == null)
            {
                return IncidentFactory.Create(IncidentsCodes.BadToken);
            }

            return null;
        }
    }
}