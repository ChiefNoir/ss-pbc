using Abstractions.IRepository;
using Abstractions.Model;
using API.Model;
using API.Security;
using BusinessService.Logic.Supervision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers.Gateway
{
    [ApiController]
    [Route("api/v1/")]
    public class Authentication : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public Authentication(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {            
            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return Task<Identity>.Run(()=> 
                {
                    var user = _userRepository.GetUser(credentials.Login, credentials.Password);

                    //var user = GetIdentity(credentials.Login, credentials.Password);
                    if (user == null)
                        throw new System.Exception("Wrong login or password");

                    var ss = new Identity
                    {
                        Login = credentials.Login,
                        Token = TokenManager.CreateToken(_configuration, CreateClaims(credentials.Login, user.Role)),
                        TokenLifeTimeMinutes = _configuration.GetSection("Token").GetValue<int>("LifeTime")
                    };

                    return ss;
                });
            });

            return new JsonResult(result);
        }

        [HttpGet("pingadmin")]
        public async Task<IActionResult> Ping1()
        {
            var token = Request.Headers["Authorization"];
            var principals = TokenManager.ValidateToken(_configuration, token);

            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return Task<string>.Run(() => 
                {
                    if (!principals.IsInRole(RoleNames.Admin))
                        throw new Exception("Denied");

                    return "pong-admin"; 
                });
            });

            return new JsonResult(result);
        }

        [HttpGet("pingdemo")]
        public async Task<IActionResult> Ping2()
        {
            var token = Request.Headers["Authorization"];
            var principals = TokenManager.ValidateToken(_configuration, token);

            var result = await Supervisor.SafeExecuteAsync(() =>
            {
                return Task<string>.Run(() => 
                {
                    if (!principals.IsInRole(RoleNames.Demo))
                        throw new Exception("Denied");

                    return "pong-demo";                 
                });
            });

            return new JsonResult(result);
        }

        private IEnumerable<Claim> CreateClaims(string login, params string[] roles)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                    };

            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, item));
            }

            return claims;
        }

    }


}
