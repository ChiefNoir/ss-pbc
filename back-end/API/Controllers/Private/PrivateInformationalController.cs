using Abstractions.Model;
using Abstractions.Supervision;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Security;
using Security.Extensions;
using System.Reflection;

namespace API.Controllers.Private
{
    [ApiController]
    [Route("api/v1/")]
    public class PrivateInformationalController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISupervisor _supervisor;

        public PrivateInformationalController(IConfiguration config, ISupervisor supervisor)
        {
            _config = config;
            _supervisor = supervisor;
        }

        [HttpGet("information")]
        public IActionResult GetIntroduction([FromHeader] string token)
        {
            var result = _supervisor.SafeExecute(token, new[] { RoleNames.Admin, RoleNames.Demo }, () =>
            {
                var claims = TokenManager.ValidateToken(_config, token);

                return new Information
                {
                    Login = claims.Identity.Name,
                    Role = string.Join(';', claims.GetRoles()),
                    ApiVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString()
                };
            });

            return new JsonResult(result);
        }
    }
}