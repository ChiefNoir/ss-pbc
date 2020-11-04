using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.DataModel
{
    [Table("category")]
    [ExcludeFromCodeCoverage]
    internal class Category
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("display_name")]
        public string DisplayName { get; set; }

        [Column("is_everything")]
        public bool IsEverything { get; set; }

        [Column("version")]
        public long Version { get; set; }
    }
}
