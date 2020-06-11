using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace API.Security
{
    public class TokenManager
    {
        public static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration.GetSection("Token").GetValue<string>("Issuer"),
                ValidateAudience = true,
                ValidAudience = configuration.GetSection("Token").GetValue<string>("Audience"),
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Token").GetValue<string>("Key"))),
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true
            };
        }

        public static string CreateToken(IConfiguration configuration, IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(
                    issuer: configuration.GetSection("Token").GetValue<string>("Issuer"),
                    audience: configuration.GetSection("Token").GetValue<string>("Audience"),
                    notBefore: DateTime.UtcNow,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(configuration.GetSection("Token").GetValue<int>("LifeTime"))),
                    signingCredentials: new SigningCredentials
                    (
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Token").GetValue<string>("Key"))),
                        configuration.GetSection("Token").GetValue<string>("SecurityAlgorithms")
                    ));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            return encodedJwt;
        }

        public static IPrincipal ValidateToken(IConfiguration configuration, string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = CreateTokenValidationParameters(configuration);

            SecurityToken validatedToken;
            return tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
        }
    }
}
