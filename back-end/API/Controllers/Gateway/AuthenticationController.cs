using Abstractions.IRepository;
using Abstractions.Model;
using Abstractions.Supervision;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Security;
using Security.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Gateway
{
    [ApiController]
    [Route("api/v1/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly ISupervisor _supervisor;

        public AuthenticationController(IConfiguration configuration, IAccountRepository userRepository, ISupervisor supervisor)
        {
            _configuration = configuration;
            _accountRepository = userRepository;
            _supervisor = supervisor;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            var result = await _supervisor.SafeExecuteAsync(async () =>
            {
                var user = await _accountRepository.GetAsync(credentials.Login, credentials.Password);

                return new Identity
                {
                    Account = user,
                    Token = TokenManager.CreateToken(_configuration, credentials.Login, user.Role),
                    TokenLifeTimeMinutes = _configuration.GetSection("Token:LifeTime").Get<int>()
                };
            });

            return new JsonResult(result);
        }

        [HttpPost("token")]
        public IActionResult Validate([FromHeader] string token)
        {
            var result = _supervisor.SafeExecute(() =>
            {
                var principal = TokenManager.ValidateToken(_configuration, token);

                if (principal == null)
                    throw new Exception("Validation failed");

                if (principal.Identity == null)
                    throw new Exception("Validation failed");

                return new Identity
                {
                    Account = new Account
                    {
                        Login = principal.Identity.Name,
                        Role = principal.GetRoles().FirstOrDefault(),
                    },
                    Token = token,
                    TokenLifeTimeMinutes = _configuration.GetSection("Token:LifeTime").Get<int>()
                };
            });

            return new JsonResult(result);
        }
    }
}