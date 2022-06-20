using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Models
{
    [ExcludeFromCodeCoverage]
    /// <summary> Introduction for the index page </summary>
    public class Introduction
    {
        /// <summary> Content </summary>
        public string Content { get; set; }

        /// <summary> Poster description </summary>
        public string PosterDescription { get; set; }

        /// <summary> Poster URL </summary>
        public string PosterUrl { get; set; }

        /// <summary> Introduction title </summary>
        public string Title { get; set; }

        /// <summary> Entity version </summary>
        public long Version { get; set; }

        /// <summary> External URLs </summary>
        public IEnumerable<ExternalUrl> ExternalUrls { get; set; }
    }
}
