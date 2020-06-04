using Abstractions.Common;
using System;
using System.Collections.Generic;

namespace Abstractions.Model
{
    public class Project : IVersion
    {
        public string Code { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string PosterUrl { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public Category Category { get; set;}

        public long Version { get; set; }

        public IEnumerable<ExternalUrl> ExternalUrls { get; set; }
    }
}
