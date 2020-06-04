using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("externalurl")]
    class ExternalUrl
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("projectcode")]
        public string ProjectCode { get; set; }

        [Required]
        [Column("url")]
        public string Url { get; set; }

        [Required]
        [Column("displayname")]
        public string DisplayName { get; set; }

        [Required]
        [Column("version")]
        public int Version { get; set; }
    }
}
