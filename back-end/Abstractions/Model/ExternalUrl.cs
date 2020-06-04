using Abstractions.Common;

namespace Abstractions.Model
{
    /// <summary> URL to the external recourses </summary>
    public class ExternalUrl : IVersion
    {
        /// <summary> Unique Id </summary>
        public int Id { get; set; }

        /// <summary> Friendly name for URL </summary>
        public string DisplayName { get; set; }

        /// <summary> URL to the external recourse </summary>
        public string Url { get; set; }

        /// <summary> Version </summary>
        public long Version { get; set; }
    }
}
