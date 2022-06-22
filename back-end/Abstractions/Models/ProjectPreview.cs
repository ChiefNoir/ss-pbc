using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Models
{
    [ExcludeFromCodeCoverage]
    /// <summary> Short version of project </summary>
    public class ProjectPreview
    {
        /// <summary> Project unique code </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary> Project category </summary>
        public Category? Category { get; set; }

        /// <summary> Short description </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary> Friendly name</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary> </summary>
        public string PosterDescription { get; set; } = string.Empty;

        /// <summary> URL for the poster </summary>
        public string? PosterUrl { get; set; }

        /// <summary> Release date </summary>
        public DateTime? ReleaseDate { get; set; }
    }
}
