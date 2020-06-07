using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("category")]
    class Category
    {
        [Key]
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
