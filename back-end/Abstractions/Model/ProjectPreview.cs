using System;

namespace Abstractions.Model
{
    public class ProjectPreview
    {
        public string Code { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string PosterUrl { get; set; }
    }
}
