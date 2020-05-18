using BusinessService.DataLayer.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.DataLayer.Model
{
    /// <summary>Full project</summary>
    public class Project : IVersion
    {
        [Key]
        public int Id { get; set; }

        /// <summary> Unique code for a project </summary>
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public Category Category { get; set; }

        public string DescriptionShort { get; set; }

        public string Description { get; set; }

        public ICollection<ExternalUrl> ExternalUrls { get; set; }

        /// <summary>Entity version</summary>
        public int Version { get; set; }
    }
}
