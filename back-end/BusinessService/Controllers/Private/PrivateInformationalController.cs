using Abstractions.IRepository;
using Abstractions.Model;
using Abstractions.Model.System;
using API.Model;
using API.Queries;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Security;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateInformationalController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _config;

        public PrivateInformationalController(IAccountRepository accountRepository, IConfiguration config)
        {
            _accountRepository = accountRepository;
            _config = config;
        }

        [HttpGet("information")]
        public async Task<IActionResult> GetIntroduction([FromHeader] string token)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin, RoleNames.Demo }, async() =>
            {
                var claims = TokenManager.ValidateToken(_config, token);

                return new Information
                {
                    Login = claims.Identity.Name,
                    Role = "TODO",
                    APIVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                };
            });

            return new JsonResult(result);
        }
    }
}
