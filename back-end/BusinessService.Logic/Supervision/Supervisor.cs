using Abstractions.IFactory;
using Abstractions.Model.System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security;
using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BusinessService.Logic.Supervision
{
    /// <summary> Utility class that provides wrapper for all things that might fail </summary>
    /// <remarks> Service <b>CAN NOT</b> throw <seealso cref="Exception"/> and die</remarks>
    public static class Supervisor
    {
        private static IConfiguration _configuration;

        public static void InitConfiguration(IConfiguration configuration)
        {
            //for Token validation
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

        /// <summary> Execute function and return its result if everything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="func"><seealso cref="Func"/> to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        public static async Task<ExecutionResult<T>> SafeExecuteAsync<T>(Func<T> func)
        {
            var result = new ExecutionResult<T>();

            try
            {
                result.Data = await Task.Run(func);
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


        /// <summary> Execute function and return its result if everything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="token">JWT token to validate</param>
        /// <param name="roles">Roles who have rights to execute function</param>
        /// <param name="func">Function to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        public static async Task<ExecutionResult<T>> SafeExecuteAsync<T>(string token, string[] roles, Func<Task<T>> func)
        {
            try
            {
                var incident = CheckToken(token, roles);

                if (incident != null)
                {
                    return new ExecutionResult<T>
                    {
                        IsSucceed = false,
                        Error = incident
                    };
                }
            }
            catch (Exception ee)
            {
                //TODO: log error
                return new ExecutionResult<T>
                {
                    IsSucceed = false,
                    Error = IncidentFactory.Create(IncidentsCodes.InternalError, ee.Message),
                };
            }

            return await SafeExecuteAsync(func);
        }

        /// <summary> Execute function and return its result if everything goes right </summary>
        /// <typeparam name="T">Type of returning result</typeparam>
        /// <param name="token">JWT token to validate</param>
        /// <param name="roles">Roles who have rights to execute function</param>
        /// <param name="func">Function to execute</param>
        /// <returns>Result of execution or ErrorMessage will have message </returns>
        public static async Task<ExecutionResult<T>> SafeExecuteAsync<T>(string token, string[] roles, Func<T> func)
        {
            try
            {
                var incident = CheckToken(token, roles);

                if (incident != null)
                {
                    return new ExecutionResult<T>
                    {
                        IsSucceed = false,
                        Error = incident
                    };
                }
            }
            catch (Exception ee)
            {
                //TODO: log error
                return new ExecutionResult<T>
                {
                    IsSucceed = false,
                    Error = IncidentFactory.Create(IncidentsCodes.InternalError, ee.Message),
                };
            }

            return await SafeExecuteAsync(func);
        }


        /// <summary> Check JWT token</summary>
        /// <param name="token">Token to validate</param>
        /// <param name="roles">Roles who have rights to execute function</param>
        /// <returns> <c>null</c> if everything is good </returns>
        private static Incident CheckToken(string token, string[] roles)
        {
            if (string.IsNullOrEmpty(token))
            {
                return IncidentFactory.Create(IncidentsCodes.AuthenticationNotProvided);
            }

            IPrincipal principal;
            try
            {
                principal = TokenManager.ValidateToken(_configuration, token);
            }
            catch (SecurityTokenException ee)
            {
                return IncidentFactory.Create(IncidentsCodes.InvalidToken); //, ee.Message);
            }

            if (principal == null || principal.Identity == null)
            {
                return IncidentFactory.Create(IncidentsCodes.InvalidToken);
            }

            if (roles == null || roles.Length == 0)
            {
                return null;
            }

            foreach (var item in roles)
            {
                if (principal.IsInRole(item))
                {
                    return null;
                }
            }

            return IncidentFactory.Create(IncidentsCodes.NotEnoughRights);
        }
    }
}