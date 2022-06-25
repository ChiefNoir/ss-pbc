using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Models
{
    [Table("account")]
    [ExcludeFromCodeCoverage]
    internal class Account
    {
        [Key]
        [Column("id")]
        public Guid? Id { get; set; }

        [Column("login")]
        public string Login { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = string.Empty;

        [Column("salt")]
        public string Salt { get; set; } = string.Empty;

        [Column("version")]
        public long Version { get; set; }
    }
}
