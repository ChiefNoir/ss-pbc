using Abstractions.API;
using Abstractions.Model.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security;
using System.Threading.Tasks;

namespace API.Controllers.Gateway
{
    public partial class GatewayController : GatewayControllerBase
    {
        public override async Task<IActionResult> LoginAsync([FromBody] Credentials credentials)
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

        public override async Task<IActionResult> ValidateAsync([FromHeader] string token)
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
