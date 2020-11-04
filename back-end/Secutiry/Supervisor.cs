using Abstractions.Model.System;
using Abstractions.Supervision;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Security
{
    public class Supervisor : ISupervisor
    {
        private readonly ILogger<Supervisor> _logger;

        public Supervisor(ILogger<Supervisor> logger)
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