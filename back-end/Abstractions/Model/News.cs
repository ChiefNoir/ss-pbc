using Abstractions.Common;

namespace Abstractions.Model
{
    public class News : IVersion
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string PosterUrl { get; set; }

        public long Version { get; set; }
    }
}
