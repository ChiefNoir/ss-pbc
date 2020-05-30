using BusinessService.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.DataLayer.Model
{
    /// <summary> URL to the external resource </summary>
    [Table("externalurl")]
    public class ExternalUrl : IVersion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("projectid")]
        public int ProjectId { get; set; }

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
