using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Model.System
{
    [ExcludeFromCodeCoverage]
    /// <summary> Wrapper for exceptional situation</summary>
    public sealed class Incident
    {
        /// <summary> Get situation message </summary>
        public string Message { get; set; }

        /// <summary> Get or set additional details</summary>
        public string Detail { get; set; }

    }
}
