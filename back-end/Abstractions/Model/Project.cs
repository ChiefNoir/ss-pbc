using Abstractions.Common;
using System;
using System.Collections.Generic;

namespace Abstractions.Model
{
    /// <summary> Full project </summary>
    public class Project : IVersion
    {
        /// <summary> Project unique code </summary>
        public string Code { get; set; }

        /// <summary> Friendly name</summary>
        public string DisplayName { get; set; }

        /// <summary> Description </summary>
        public string Description { get; set; }

        /// <summary> URL for the poster </summary>
        public string PosterUrl { get; set; }

        /// <summary> Release date </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary> Project category </summary>
        public Category Category { get; set;}

        /// <summary> Version </summary>
        public long Version { get; set; }

        /// <summary> URL for the external recourses for the project </summary>
        public IEnumerable<ExternalUrl> ExternalUrls { get; set; }
    }
}
