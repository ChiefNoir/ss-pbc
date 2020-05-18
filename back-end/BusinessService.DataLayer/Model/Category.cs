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

        public int Version { get; set; }
    }
}
