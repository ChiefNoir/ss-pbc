using Abstractions.Models;

namespace SSPBC.Models
{
    public class Identity
    {
        /// <summary> Account id </summary>
        public Guid AccountId { get; set; }

        /// <summary> Account login </summary>
        public string Login { get; set; } = string.Empty;

        /// <summary> Account role </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary> Account identification  token </summary>
        public string? Token { get; set; }

        /// <summary> Tokens lifespan </summary>
        public int TokenLifeTimeMinutes { get; set; }
    }
}
