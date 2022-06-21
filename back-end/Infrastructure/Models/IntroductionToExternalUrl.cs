using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Models
{
    [Table("introduction_to_external_url")]
    [ExcludeFromCodeCoverage]
    internal class IntroductionToExternalUrl
    {
        [Column("introduction_id")]
        public Guid IntroductionId { get; set; }

        [Column("external_url_id")]
        public Guid ExternalUrlId { get; set; }

        public ExternalUrl ExternalUrl { get; set; }
    }
}
