using BusinessService.DataLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.DataLayer.Model
{
    /// <summary> URL to the external resource </summary>
    public class ExternalUrl : IVersion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public int Version { get; set; }
    }
}
