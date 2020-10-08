using System.Diagnostics.CodeAnalysis;

namespace API.Model
{
    [ExcludeFromCodeCoverage]
    public class Information
    {
        public string Login { get; set; }

        public string Role { get; set; }
        
        public string ApiVersion { get; set; }
    }
}
