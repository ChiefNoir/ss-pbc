using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.DataModel
{
    [Table("project_to_external_url")]
    [ExcludeFromCodeCoverage]
    internal class IntroductionToExternalUrl
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
