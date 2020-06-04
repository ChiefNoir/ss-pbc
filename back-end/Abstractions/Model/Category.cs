using Abstractions.Common;

namespace Abstractions.Model
{
    public class Category : IVersion
    {
        public string Code { get; set; }
        
        public string DisplayName { get; set; }
        
        public bool IsEverything { get; set; }

        public long Version { get; set; }
    }
}
