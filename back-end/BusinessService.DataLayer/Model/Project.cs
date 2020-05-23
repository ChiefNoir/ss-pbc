using BusinessService.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessService.DataLayer.Model
{
    /// <summary>Full project</summary>
    [Table("project")]
    public class Project : IVersion
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary> Unique code for a project </summary>
        [Required]
        [Column("code")]
        public string Code { get; set; }

        [Required]
        [Column("displayname")]
        public string DisplayName { get; set; }

        [Column("releasedate")]
        public DateTime? ReleaseDate { get; set; }

        [Column("imageurl")]
        public string ImageUrl { get; set; }

        [Column("categoryid"), ForeignKey("category")]
        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }

        [Column("descriptionshort")]
        public string DescriptionShort { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("version")]
        /// <summary>Entity version</summary>
        public int Version { get; set; }

        public ICollection<ExternalUrl> ExternalUrls { get; set; }

    }
}
