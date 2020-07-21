using System.Collections;
using System.Collections.Generic;
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

    [Table("project_to_external_url")]
    class ProjectExternalUrl
    {
        [Column("project_id")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [Column("external_url_id")]
        public int ExternalUrlId { get; set; }
        public ExternalUrl ExternalUrl { get; set; }
    }

    [Table("introduction_to_external_url")]
    class IntroductionExternalUrl
    {
        [Column("introduction_id")]
        public int IntroductionId { get; set; }

        public Introduction Introduction { get; set; }

        [Column("external_url_id")]
        public int ExternalUrlId { get; set; }
        public ExternalUrl ExternalUrl { get; set; }
    }
}
