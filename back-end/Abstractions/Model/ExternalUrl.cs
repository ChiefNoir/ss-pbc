using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Model
{
    [ExcludeFromCodeCoverage]
    /// <summary> URL to the external resources </summary>
    public class ExternalUrl
    {
        /// <summary> Unique id or <c>null</c> if external URL is brand new </summary>
        public int? Id { get; set; }

        /// <summary> Friendly name for URL </summary>
        public string DisplayName { get; set; }

        /// <summary> URL to the external recourse </summary>
        public string Url { get; set; }

        /// <summary> Entity version </summary>
        public long Version { get; set; }
    }
}