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

        [Column("displayname")]
        public string DisplayName { get; private set; }

        [Column("iseverything")]
        public bool IsEverything { get; private set; }

        [Column("version")]
        public long Version { get; private set; }

        [Column("totalprojects")]
        public int TotalProjects { get; private set; }
    }
}
