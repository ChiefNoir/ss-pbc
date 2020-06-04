using Abstractions.Common;

namespace Abstractions.Model
{
    /// <summary> Project category</summary>
    public class Category : IVersion
    {
        /// <summary> Unique category code</summary>
        public string Code { get; set; }
        
        /// <summary> Friendly category name </summary>
        public string DisplayName { get; set; }
        
        // TODO: maybe I should remove it
        /// <summary> Is category technical for filtering by all</summary>
        public bool IsEverything { get; set; }

        /// <summary> Version </summary>
        public long Version { get; set; }
    }
}
