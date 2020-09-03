using Abstractions.IRepository;
using Abstractions.Model;
using API.Model;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Security;
using Security.Extensions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers.Gateway
{
    [ApiController]
    [Route("api/v1/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _userRepository;

        public AuthenticationController(IConfiguration configuration, IAccountRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            var result = await Supervisor.SafeExecuteAsync(async () =>
            {
                var user = await _userRepository.Get(credentials.Login, credentials.Password);

                if (user == null)
                    throw new Exception("Wrong user or password");

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
        public async Task<IActionResult> Validate([FromHeader] string token)
        {
            var result = await Supervisor.SafeExecuteAsync(async () =>
            {
                var principal = TokenManager.ValidateToken(_configuration, token);

                if (principal == null)
                    throw new Exception("Validate failed");

                return new Identity
                {
                    Account = principal == null ? null : new Account
                    {
                        Login = principal?.Identity.Name,
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