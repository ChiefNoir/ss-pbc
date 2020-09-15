using Abstractions.Model;
using API.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Security;
using Security.Extensions;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateInformationalController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PrivateInformationalController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("information")]
        public async Task<IActionResult> GetIntroduction([FromHeader] string token)
        {
            var result = await Supervisor.SafeExecuteAsync(token, new[] { RoleNames.Admin, RoleNames.Demo }, () =>
            {
                var claims = TokenManager.ValidateToken(_config, token);

                return new Information
                {
                    Login = claims.Identity.Name,
                    Role = string.Join(';', claims.GetRoles()),
                    APIVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                };
            });

            return new JsonResult(result);
        }
    }
}