using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Models
{
    [ExcludeFromCodeCoverage]
    /// <summary> Introduction for the index page </summary>
    public class Introduction
    {
        /// <summary> Content </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary> Poster description </summary>
        public string PosterDescription { get; set; } = string.Empty;

        /// <summary> Poster URL </summary>
        public string PosterUrl { get; set; } = string.Empty;

        /// <summary> Introduction title </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary> Entity version </summary>
        public long Version { get; set; }

        /// <summary> External URLs </summary>
        public IEnumerable<ExternalUrl> ExternalUrls { get; set; } = Enumerable.Empty<ExternalUrl>();
    }
}
