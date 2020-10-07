using Abstractions.ISecurity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Security
{
    /// <summary> JWT Toke manager </summary>
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration _configuration;

        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary> Create JWT token </summary>
        /// <param name="login"> Login </param>
        /// <param name="roles"> Roles </param>
        /// <returns>JWT token</returns>
        public string CreateToken(string login, params string[] roles)
        {
            if(string.IsNullOrEmpty(login))
                throw new ArgumentException("Login can't be null or empty", nameof(login));

            if (roles == null || !roles.Any() || roles.All(x => string.IsNullOrEmpty(x)))
                throw new ArgumentException("Roles can't be null or empty", nameof(roles));

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            foreach (var item in roles.Where(x => !string.IsNullOrEmpty(x)))
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, item));
            }

            var jwt = new JwtSecurityToken
            (
                issuer: _configuration.GetSection("Token:Issuer").Get<string>(),
                audience: _configuration.GetSection("Token:Audience").Get<string>(),
                notBefore: DateTime.UtcNow,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_configuration.GetSection("Token:LifeTime").Get<int>())),
                signingCredentials: new SigningCredentials
                (
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Token:Key").Get<string>())),
                    _configuration.GetSection("Token:SecurityAlgorithms").Get<string>()
                )
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary> Create <seea cref="TokenValidationParameters"/> parameters </summary>
        /// <returns> <seea cref="TokenValidationParameters"/> </returns>
        public static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = configuration.GetSection("Token:ValidateIssuer").Get<bool>(),
                ValidateAudience = configuration.GetSection("Token:ValidateAudience").Get<bool>(),
                ValidateLifetime = configuration.GetSection("Token:ValidateLifetime").Get<bool>(),
                ValidateIssuerSigningKey = configuration.GetSection("Token:ValidateIssuerSigningKey").Get<bool>(),
                
                ValidIssuer = configuration.GetSection("Token:Issuer").Get<string>(),
                ValidAudience = configuration.GetSection("Token:Audience").Get<string>(),
                IssuerSigningKey = new SymmetricSecurityKey
                (
                    Encoding.UTF8.GetBytes(configuration.GetSection("Token:Key").Get<string>())
                ),
                RequireExpirationTime = configuration.GetSection("Token:RequireExpirationTime").Get<bool>(),
                ClockSkew = TimeSpan.FromMinutes
                (
                    configuration.GetSection("Token:ClockSkewMinutes").Get<int>()
                )
            };
        }

        /// <summary> Validate JWT token </summary>
        /// <param name="token">Token to validate</param>
        /// <returns> <see cref="IPrincipal"/></returns>
        public IPrincipal ValidateToken(string token)
        {
            //TODO: extend validation

            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token can't be null or empty", nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = CreateTokenValidationParameters(_configuration);

            var ff = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken toke);

            var dd = toke.Id;

            return ff;
        }
    }
}
