using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel
{
    [Table("project")]
    class Project
    {
        [Key]
        [Column("code")]
        public string Code { get; set; }

        [Column("display_name")]
        public string DisplayName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("description_short")]
        public string DescriptionShort { get; set; }

        [Column("poster_url")]
        public string PosterUrl { get; set; }

        [Column("poster_description")]
        public string PosterDescription { get; set; }

        [Column("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [Column("category_code")]
        public string CategoryCode { get; set; }

        public Category Category { get; set; }

        [Column("version")]
        public long Version { get; set; }

        public ICollection<ExternalUrl> ExternalUrls { get; set; }

    }
}
