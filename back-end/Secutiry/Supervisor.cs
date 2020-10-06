using Abstractions.IRepository;
using Abstractions.Model.System;
using Abstractions.Supervision;
using Microsoft.IdentityModel.Tokens;
using Security.Resources;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Abstractions.ISecurity;
using System.Security;
using System.Linq;
using Security.Extensions;

namespace Security
{
    public class Supervisor : ISupervisor
    {
        private readonly ITokenManager _tokenManager;

        public Supervisor(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
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
                CheckToken(token, roles);
                return await SafeExecuteAsync(func);
            }
            catch (Exception ee)
            {
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
        }


        public ExecutionResult<T> SafeExecute<T>(string token, string[] roles, Func<T> func)
        {
            try
            {
                CheckToken(token, roles);
                return SafeExecute(func);
            }
            catch (Exception ee)
            {
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
        }


        /// <summary> Check JWT token</summary>
        /// <param name="token">Token to validate</param>
        /// <param name="roles">Roles who have rights to execute function</param>
        /// <returns> <c>null</c> if everything is good </returns>
        private void CheckToken(string token, string[] roles)
        {
            if (string.IsNullOrEmpty(token))
                throw new SecurityException(TextMessages.AuthenticationNotProvided);


            IPrincipal principal;
            try
            {
                principal = _tokenManager.ValidateToken(token);
            }
            catch (SecurityTokenException ee)
            {
                throw new SecurityException(TextMessages.InvalidToken, ee);
            }

            if (principal?.Identity == null)
                throw new SecurityException(TextMessages.InvalidToken);

            if (!principal.GetRoles().Any())
                throw new SecurityException(TextMessages.InvalidToken);

            if (roles == null || !roles.Any())
                return;

            foreach (var item in roles)
            {
                if (principal.IsInRole(item))
                {
                    return;
                }
            }

            throw new SecurityException(TextMessages.AccessDenied);
        }
    }

}