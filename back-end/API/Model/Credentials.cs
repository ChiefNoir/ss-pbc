using System.Diagnostics.CodeAnalysis;

namespace API.Model
{
    [ExcludeFromCodeCoverage]
    /// <summary> Users credentials</summary>
    public class Credentials
    {
        /// <summary> User name </summary>
        public string Login { get; set; }

        /// <summary> Password </summary>
        public string Password { get; set; }
    }
}