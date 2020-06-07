using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("news")]
    class News
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("poster_url")]
        public string PosterUrl { get; set; }

        [Column("version")]
        public long Version { get; set; }
    }
}
