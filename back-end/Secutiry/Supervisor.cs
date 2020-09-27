using Abstractions.IRepository;
using Abstractions.Model.System;
using Abstractions.Supervision;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security.Resources;
using System;
using System.Security.Principal;
using System.Threading.Tasks;


namespace Security
{
    public class Supervisor : ISupervisor
    {
        private static IConfiguration _configuration;
        private static ILogRepository _logRepository;

        public Supervisor(IConfiguration configuration, ILogRepository logRepository)
        {
            _configuration = configuration;
            _logRepository = logRepository;
        }


        public async Task<ExecutionResult<T>> SafeExecuteAsync<T>(Func<Task<T>> func)
        {
            var result = new ExecutionResult<T>();

            try
            {
                result.Data = await func();
                result.IsSucceed = true;
            }
            catch (Exception ee)
            {
                _logRepository.LogError(ee);

                result.IsSucceed = false;
                result.Error = new Incident
                {
                    Message = ee.Message,
                    Detail = ee.InnerException?.Message
                };
            }

            return result;
        }


        public ExecutionResult<T> SafeExecute<T>(Func<T> func)
        {
            var result = new ExecutionResult<T>();

            try
            {
                result.Data = func();
                result.IsSucceed = true;
            }
            catch (Exception ee)
            {
                _logRepository.LogError(ee);

                result.IsSucceed = false;
                result.Error = new Incident
                {
                    Message = ee.Message,
                    Detail = ee.InnerException?.Message
                };
            }

            return result;
        }


        public async Task<ExecutionResult<T>> SafeExecuteAsync<T>(string token, string[] roles, Func<Task<T>> func)
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
                _logRepository.LogError(ee);

                return new ExecutionResult<T>
                {
                    IsSucceed = false,
                    Error = new Incident
                    {
                        Message = ee.Message,
                        Detail = ee.InnerException?.Message
                    }
                };
            }

            return await SafeExecuteAsync(func);
        }


        public ExecutionResult<T> SafeExecute<T>(string token, string[] roles, Func<T> func)
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
                _logRepository.LogError(ee);

                return new ExecutionResult<T>
                {
                    IsSucceed = false,
                    Error = new Incident
                    {
                        Message = ee.Message,
                        Detail = ee.InnerException?.Message
                    }
                };
            }

            return SafeExecute(func);
        }


        //TODO: make this an secException
        /// <summary> Check JWT token</summary>
        /// <param name="token">Token to validate</param>
        /// <param name="roles">Roles who have rights to execute function</param>
        /// <returns> <c>null</c> if everything is good </returns>
        private static Incident CheckToken(string token, string[] roles)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new Incident
                {
                    Message = TextMessages.AuthenticationNotProvided
                };

            }

            IPrincipal principal;
            try
            {
                principal = TokenManager.ValidateToken(_configuration, token);
            }
            catch (SecurityTokenException ee)
            {
                return new Incident
                {
                    Message = TextMessages.InvalidToken,
                    Detail = ee.Message
                };
            }

            if (principal?.Identity == null)
            {
                return new Incident
                {
                    Message = TextMessages.InvalidToken
                };
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

            return new Incident
            {
                Message = TextMessages.AccessDenied
            };
        }
    }

}