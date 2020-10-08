using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.DataModel
{
    [Table("categories_with_projects_total_v")]
    [ExcludeFromCodeCoverage]
    internal class CategoryWithTotalProjects
    {
        [Key]
        [Column("id")]
        public int Id { get; private set; }

        [Column("code")]
        public string Code { get; private set; }

        [Column("display_name")]
        public string DisplayName { get; private set; }

        [Column("is_everything")]
        public bool IsEverything { get; private set; }

        [Column("total_projects")]
        public int TotalProjects { get; private set; }

        [Column("version")]
        public long Version { get; private set; }
    }
}