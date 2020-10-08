using System;
using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Model
{
    [ExcludeFromCodeCoverage]
    /// <summary> Short version of project </summary>
    public class ProjectPreview
    {
        /// <summary> Project unique code </summary>
        public string Code { get; set; }

        /// <summary> Project category </summary>
        public Category Category { get; set; }

        /// <summary> Short description </summary>
        public string Description { get; set; }

        /// <summary> Friendly name</summary>
        public string DisplayName { get; set; }

        /// <summary> </summary>
        public string PosterDescription { get; set; }

        /// <summary> URL for the poster </summary>
        public string PosterUrl { get; set; }

        /// <summary> Release date </summary>
        public DateTime? ReleaseDate { get; set; }
    }
}