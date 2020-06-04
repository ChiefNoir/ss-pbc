using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.DataModel
{
    [Table("project")]
    class Project
    {
        [Key]
        [Column("code")]
        public string Code { get; set; }

        [Column("displayname")]
        public string DisplayName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("descriptionshort")]
        public string DescriptionShort { get; set; }

        [Column("posterurl")]
        public string PosterUrl { get; set; }

        [Column("releasedate")]
        public DateTime? ReleaseDate { get; set; }

        [Column("categorycode")]
        public string CategoryCode { get; set; }

        public Category Category { get; set; }

        [Column("version")]
        public long Version { get; set; }

        public ICollection<ExternalUrl> ExternalUrls { get; set; }

    }
}
