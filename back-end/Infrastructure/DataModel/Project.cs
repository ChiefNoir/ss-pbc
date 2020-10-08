using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.DataModel
{
    [Table("project")]
    [ExcludeFromCodeCoverage]
    internal class Project
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("description_short")]
        public string DescriptionShort { get; set; }

        [Column("display_name")]
        public string DisplayName { get; set; }

        [Column("poster_description")]
        public string PosterDescription { get; set; }

        [Column("poster_url")]
        public string PosterUrl { get; set; }

        [Column("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [Column("version")]
        public long Version { get; set; }

        public Category Category { get; set; }

        public ICollection<ProjectExternalUrl> ExternalUrls { get; set; }

        public ICollection<GalleryImage> GalleryImages { get; set; }

    }
}