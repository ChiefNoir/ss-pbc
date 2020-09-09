using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("gallery_image")]
    internal class GalleryImage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("extra_url")]
        public string ExtraUrl { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("project_id")]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        [Column("version")]
        public long Version { get; set; }
    }
}
