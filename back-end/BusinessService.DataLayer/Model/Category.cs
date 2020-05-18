using BusinessService.DataLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.DataLayer.Model
{
    /// <summary> Project category </summary>
    public class Category : IVersion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        public string DisplayName { get; set; }

        public string ImageUrl { get; set; }

        /// <summary> If <c>true</c> then, this is technical category for searching for projects in any category</summary>
        public bool IsEverything { get; set; }

        public int Version { get; set; }
    }
}
