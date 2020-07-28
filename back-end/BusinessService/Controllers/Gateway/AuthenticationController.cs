using Abstractions.IRepository;
using API.Model;
using API.Security;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
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
                    Login = user.Login,
                    Token = TokenManager.CreateToken(_configuration, credentials.Login, user.Role),
                    TokenLifeTimeMinutes = _configuration.GetSection("Token").GetValue<int>("LifeTime")
                };
            });

            return new JsonResult(result);
        }
    }
}