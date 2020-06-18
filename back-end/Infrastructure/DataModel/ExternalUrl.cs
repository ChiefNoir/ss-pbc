using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("external_url")]
    class ExternalUrl
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("project_id")]
        public int ProjectId { get; set; }

        [Required]
        [Column("url")]
        public string Url { get; set; }

        [Required]
        [Column("display_name")]
        public string DisplayName { get; set; }

        [Required]
        [Column("version")]
        public int Version { get; set; }
    }
}
