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
using Microsoft.Extensions.Logging;

namespace Security
{
    public class Supervisor : ISupervisor
    {
        private readonly ILogger<Supervisor> _logger;

        public Supervisor(ITokenManager tokenManager, ILogger<Supervisor> logger)
        {
            _logger = logger;
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
                _logger.LogError(ee, "Exception in public call");

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
                _logger.LogError(ee, "Exception in public call");

                result.IsSucceed = false;
                result.Error = new Incident
                {
                    Message = ee.Message,
                    Detail = ee.InnerException?.Message
                };
            }

            return result;
        }

    }

}