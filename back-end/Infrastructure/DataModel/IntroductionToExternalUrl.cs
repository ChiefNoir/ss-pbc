using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.DataModel
{
    [Table("project_to_external_url")]
    class IntroductionToExternalUrl
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("introduction_id")]
        public int IntroductionId { get; set; }

        [Column("external_url_id")]
        public int ExternalUrlId { get; set; }
    }
}
