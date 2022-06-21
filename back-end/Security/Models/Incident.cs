using System.Diagnostics.CodeAnalysis;

namespace Security.Models
{
    [ExcludeFromCodeCoverage]
    /// <summary> Wrapper for exceptional situation</summary>
    public sealed class Incident
    {
        /// <summary> Get situation message </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary> Get or set additional details</summary>
        public string? Detail { get; set; }

    }
}
