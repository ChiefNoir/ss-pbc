using BusinessService.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.DataLayer.Model
{
    /// <summary> Project category </summary>
    [Table("category")]
    public class Category : IVersion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Required]
        [Column("displayname")]
        public string DisplayName { get; set; }

        [Column("imageurl")]
        public string ImageUrl { get; set; }

        /// <summary> If <c>true</c> then, this is technical category for searching for projects in any category</summary>
        [Column("iseverything")]
        public bool IsEverything { get; set; }

        [Required]
        [Column("version")]
        public int Version { get; set; }
    }
}
