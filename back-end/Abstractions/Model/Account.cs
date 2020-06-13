namespace Abstractions.Model
{
    /// <summary> Account </summary>
    public class Account
    {
        /// <summary> Login </summary>
        public string Login { get; set; }

        /// <summary> Password as a hex string </summary>
        public string Password { get; set; }
        
        /// <summary> Role <seealso cref="RoleNames"/></summary>
        public string Role { get; set; }
    }
}
