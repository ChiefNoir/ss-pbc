using BusinessService.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.DataLayer.Model
{
    /// <summary>Settings stored in db</summary>
    [Table("serversetting")]
    public class ServerSetting : IVersion
    {
        /// <summary>Setting key </summary>
        [Key]
        [Column("key")]
        public string Key { get; set; }

        /// <summary> Friendly and understandable setting name </summary>
        [Required]
        [Column("displayname")]
        public string DisplayName { get; set; }

        /// <summary> Value. It can be anything of simple type: <seealso cref="string"/>, <seealso cref="int"/>, etc </summary>
        [Column("value")]
        public string Value { get; set; }

        [Required]
        [Column("version")]
        public int Version { get; set; }
    }
}
