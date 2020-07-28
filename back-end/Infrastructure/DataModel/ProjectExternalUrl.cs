using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("project_to_external_url")]
    internal class ProjectExternalUrl
    {
        [Column("project_id")]
        public int ProjectId { get; set; }

        [Column("external_url_id")]
        public int ExternalUrlId { get; set; }

        public Project Project { get; set; }

        public ExternalUrl ExternalUrl { get; set; }
    }
}
