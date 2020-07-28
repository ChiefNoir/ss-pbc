using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("introduction_to_external_url")]
    internal class IntroductionExternalUrl
    {
        [Column("external_url_id")]
        public int ExternalUrlId { get; set; }

        [Column("introduction_id")]
        public int IntroductionId { get; set; }

        public ExternalUrl ExternalUrl { get; set; }

        public Introduction Introduction { get; set; }
    }
}