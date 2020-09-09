using Abstractions.Common;

namespace Abstractions.Model
{
    public class GalleryImage : IVersion
    {
        /// <summary> Unique id or <c>null</c> if image is brand new </summary>
        public int? Id { get; set; }

        /// <summary> Extra URL (to the full sized image, video, etc) </summary>
        public string ExtraUrl { get; set; }

        /// <summary> URL to the image for display</summary>
        public string ImageUrl { get; set; }

        /// <summary> Entity version </summary>
        public long Version { get; set; }
    }
}
