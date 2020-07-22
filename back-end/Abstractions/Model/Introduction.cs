using Abstractions.Common;
using System.Collections.Generic;

namespace Abstractions.Model
{
    public class Introduction : IVersion
    {        
        public string Content { get; set; }

        public string Title { get; set; }

        public string PosterUrl { get; set; }

        public string PosterDescription { get; set; }

        public IEnumerable<ExternalUrl> ExternalUrls { get; set; }

        public long Version { get; set; }
    }
}
