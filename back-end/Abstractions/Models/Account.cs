using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Models
{
    [ExcludeFromCodeCoverage]
    /// <summary> Account </summary>
    public class Account
    {
        /// <summary> Unique id or <c>null</c> if account is brand new </summary>
        public Guid? Id { get; set; }

        /// <summary> Login </summary>
        public string Login { get; set; } = string.Empty;

        /// <summary> Password as a hex string </summary>
        public string? Password { get; set; }

        /// <summary> Role <seealso cref="RoleNames"/></summary>
        public string Role { get; set; } = string.Empty;

        /// <summary> Entity version </summary>
        public long Version { get; set; }
    }
}
