using Abstractions.IRepositories;
using Abstractions.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security;
using Security.Models;
using SSPBC.Models;

namespace SSPBC.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/")]
    public class GatewayController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly Supervisor _supervisor;
        private readonly ITokenManager _tokenManager;

        public GatewayController(IConfiguration configuration, IAccountRepository accountRepository, Supervisor supervisor, ITokenManager tokenManager)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
            _supervisor = supervisor;
            _tokenManager = tokenManager;
        }

        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpPost("login")]
        public async Task<ActionResult<ExecutionResult<Identity>>> LoginAsync([FromBody] Credentials credentials)
        {
            var result = await _supervisor.SafeExecuteAsync(async () =>
            {
                var user = await _accountRepository.GetAsync(credentials.Login, credentials.Password);

                return new Identity
                {
                    Account = user,
                    Token = _tokenManager.CreateToken(user.Login, user.Role),
                    TokenLifeTimeMinutes = _configuration.GetSection("Token:LifeTime").Get<int>()
                };
            });

            return result;
        }
    }
}
