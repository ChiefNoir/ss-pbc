using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("categories_with_projects_total_v")]
    class CategoryWithTotalProjects
    {
        [Key]
        [Column("code")]
        public string Code { get; private set; }

        [Column("display_name")]
        public string DisplayName { get; private set; }

        [Column("is_everything")]
        public bool IsEverything { get; private set; }

        [Column("version")]
        public long Version { get; private set; }

        [Column("total_projects")]
        public int TotalProjects { get; private set; }
    }
}
