using Abstractions.Common;

namespace Abstractions.Model
{
    /// <summary> Account </summary>
    public class Account : IVersion
    {
        public int? Id { get; set; }

        /// <summary> Login </summary>
        public string Login { get; set; }

        /// <summary> Password as a hex string </summary>
        public string Password { get; set; }
        
        /// <summary> Role <seealso cref="RoleNames"/></summary>
        public string Role { get; set; }

        public long Version { get; set; }
    }
}
