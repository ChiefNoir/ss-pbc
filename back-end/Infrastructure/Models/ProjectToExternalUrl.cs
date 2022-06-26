using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Models
{
    [Table("project_to_external_url")]
    [ExcludeFromCodeCoverage]
    internal class ProjectToExternalUrl
    {
        [Column("project_id")]
        public Guid ProjectId { get; set; }

        [Column("external_url_id")]
        public Guid ExternalUrlId { get; set; }

        public Project? Project { get; set; }

        public ExternalUrl? ExternalUrl { get; set; }
    }
}
