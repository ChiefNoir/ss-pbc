using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Security
{
    [ExcludeFromCodeCoverage]
    /// <summary> User credentials</summary>
    public class Credentials
    {
        /// <summary> Get/set login </summary>
        public string Login { get; set; }

        /// <summary> Get/set password </summary>
        public string Password { get; set; }

        /// <summary> Create credentials</summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        public Credentials(string login, string password)
        {
            Login = login;
            Password = password;
        }   
    }
}
