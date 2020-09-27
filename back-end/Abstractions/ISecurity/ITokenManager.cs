using System.Security.Principal;

namespace Abstractions.ISecurity
{
    /// <summary> Token manager </summary>
    public interface ITokenManager
    {
        /// <summary> Create token </summary>
        /// <param name="login"> Login </param>
        /// <param name="roles"> Roles </param>
        /// <returns>JWT token</returns>
        string CreateToken(string login, params string[] roles);

        /// <summary> Validate token </summary>
        /// <param name="token">Token to validate</param>
        /// <returns> <see cref="IPrincipal"/></returns>
        IPrincipal ValidateToken(string token);
    }
}
