using Abstractions.Common;

namespace Abstractions.Model
{
    public class ExternalUrl : IVersion
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Url { get; set; }

        public long Version { get; set; }
    }
}
