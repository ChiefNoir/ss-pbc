using BusinessService.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.DataLayer.Model
{
    [Table("serversetting")]
    public class ServerSetting : IVersion
    {
        [Key]
        [Column("key")]
        public string Key { get; set; }

        [Required]
        [Column("displayname")]
        public string DisplayName { get; set; }

        [Column("value")]
        public string Value { get; set; }

        [Required]
        [Column("version")]
        public int Version { get; set; }
    }
}
