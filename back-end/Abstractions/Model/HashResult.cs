using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Model
{
    [ExcludeFromCodeCoverage]
    /// <summary> Hash result</summary>
    public class HashResult
    {
        /// <summary> Hashed string as hex string </summary>
        public string HexHash { get; set; }

        /// <summary> Salt as a hex string </summary>
        public string HexSalt { get; set; }
    }
}
