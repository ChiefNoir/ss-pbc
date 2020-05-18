using BusinessService.DataLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BusinessService.DataLayer.Model
{
    /// <summary> Informational blocks for index page </summary>
    public class Introduction : IVersion
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public string ImageUrl { get; set; }

        public int Version { get; set; }
    }
}
