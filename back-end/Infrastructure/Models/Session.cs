using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Models
{
    [ExcludeFromCodeCoverage]
    [Table("session")]
    internal class Session
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("account_id")]
        public Guid AccountId { get; set; }

        [Column("token")]
        public string Token { get; set; } = string.Empty;

        [Column("fingerprint")]
        public string Fingerprint { get; set; } = string.Empty;

        public Account? Account { get; set; }
    }
}
