using Abstractions.Common;

namespace Abstractions.Model
{
    /// <summary> News </summary>
    public class News : IVersion
    {
        /// <summary> Unique Id</summary>
        public int Id { get; set; }

        /// <summary> New title </summary>
        public string Title { get; set; }

        /// <summary> Content </summary>
        public string Content { get; set; }

        /// <summary> URL to the poster</summary>
        public string PosterUrl { get; set; }

        /// <summary> Short description for the poster (mostly for the image alt-text) </summary>
        public string PosterDescription { get; set; }

        /// <summary> Version </summary>
        public long Version { get; set; }
    }
}
