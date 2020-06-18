using Abstractions.Common;

namespace Abstractions.Model
{
    /// <summary> Project category</summary>
    public class Category : IVersion
    {
        public int? Id { get; set; }

        /// <summary> Unique category code</summary>
        public string Code { get; set; }
        
        /// <summary> Friendly category name </summary>
        public string DisplayName { get; set; }
        
        // TODO: maybe I should remove it
        /// <summary> Is category technical for filtering by all</summary>
        public bool IsEverything { get; set; }

        /// <summary> Get total projects in this category </summary>
        public int TotalProjects { get; set; }

        /// <summary> Version </summary>
        public long Version { get; set; }
    }
}
