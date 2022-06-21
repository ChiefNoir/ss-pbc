using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Models
{
    [ExcludeFromCodeCoverage]
    /// <summary> URL to the external resources </summary>
    public class ExternalUrl
    {
        /// <summary> Unique id or <c>null</c> if external URL is brand new </summary>
        public Guid? Id { get; set; }

        /// <summary> Friendly name for URL </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary> URL to the external recourse </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary> Entity version </summary>
        public long Version { get; set; }
    }
}
