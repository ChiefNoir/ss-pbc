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

        /// <summary> Users fingerprint </summary>
        public string Fingerprint { get; set; }

        /// <summary> Create credentials</summary>
        /// <param name="login">Login</param>
        /// <param name="password">Password</param>
        /// <param name="fingerprint">Fingerprint</param>
        public Credentials(string login, string password, string fingerprint)
        {
            Login = login;
            Password = password;
            Fingerprint = fingerprint;
        }
    }
}
