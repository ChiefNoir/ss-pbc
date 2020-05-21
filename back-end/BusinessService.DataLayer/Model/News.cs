using BusinessService.Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.DataLayer.Model
{
    /// <summary> News block for index page </summary>
    [Table("news")]
    public class News : IVersion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("imageurl")]
        public string ImageUrl { get; set; }

        [Required]
        [Column("version")]
        public int Version { get; set; }
    }
}
