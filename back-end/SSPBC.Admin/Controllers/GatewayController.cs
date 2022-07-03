using Abstractions.RepositoryPrivate;
using Abstractions.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security;
using Security.Models;
using SSPBC.Admin.Models;

namespace SSPBC.Admin.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/admin/v{version:apiVersion}/")]
    public class GatewayController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly Supervisor _supervisor;
        private readonly ITokenManager _tokenManager;
        private readonly ISessionRepository _sessionRepository;

        public GatewayController(IConfiguration configuration,
                                 IAccountRepository accountRepository,
                                 ITokenManager tokenManager,
                                 ISessionRepository sessionRepository,
                                 Supervisor supervisor)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
            _supervisor = supervisor;
            _tokenManager = tokenManager;
            _sessionRepository = sessionRepository;
        }

        [AllowAnonymous]
        [ApiVersion("1.0")]
        [HttpPost("login")]
        public async Task<ActionResult<ExecutionResult<Identity>>> LoginAsync([FromBody] Credentials credentials)
        {
            var result = await _supervisor.SafeExecuteAsync(async () =>
            {
                var account = await _accountRepository.GetAsync(credentials.Login, credentials.Password);
                var token = _tokenManager.CreateToken(account.Login, account.Role);

                await _sessionRepository.SaveSessionAsync(account, token, credentials.Fingerprint);

                return new Identity
                {
                    AccountId = account.Id!.Value,
                    Login = account.Login,
                    Role = account.Role,
                    Token = token,
                    TokenLifeTimeMinutes = _configuration.GetSection("Token:LifeTime").Get<int>()
                };
            });

            return result;
        }
    }
}
