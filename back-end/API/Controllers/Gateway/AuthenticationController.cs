using Abstractions.IRepository;
using Abstractions.Supervision;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.ISecurity;
using System.Security;

namespace API.Controllers.Gateway
{
    [ApiController]
    [Route("api/v1/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenManager _tokenManager;
        private readonly ISupervisor _supervisor;

        public AuthenticationController(IConfiguration configuration, IAccountRepository userRepository, ISupervisor supervisor, ITokenManager tokenManager)
        {
            _configuration = configuration;
            _accountRepository = userRepository;
            _supervisor = supervisor;
            _tokenManager = tokenManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] Credentials credentials)
        {
            var result = await _supervisor.SafeExecuteAsync(async () =>
            {
                var user = await _accountRepository.GetAsync(credentials.Login, credentials.Password);

                return new Identity
                {
                    Account = user,
                    Token = _tokenManager.CreateToken(credentials.Login, user.Role),
                    TokenLifeTimeMinutes = _configuration.GetSection("Token:LifeTime").Get<int>()
                };
            });

            return new JsonResult(result);
        }

        [HttpPost("token")]
        public async Task<IActionResult> ValidateAsync([FromHeader] string token)
        {
            var result = await _supervisor.SafeExecuteAsync(async () =>
            {
                var principal = _tokenManager.ValidateToken(token);

                var account = await _accountRepository.GetAsync(principal?.Identity?.Name);
                if (account == null)
                    throw new SecurityException("Validation failed");

                return new Identity
                {
                    Account = account,
                    Token = token,
                    TokenLifeTimeMinutes = _configuration.GetSection("Token:LifeTime").Get<int>()
                };
            });

            return new JsonResult(result);
        }
    }
}