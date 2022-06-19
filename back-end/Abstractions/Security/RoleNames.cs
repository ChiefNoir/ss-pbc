using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.Security
{
    [ExcludeFromCodeCoverage]
    public static class RoleNames
    {
        public const string Admin = "admin";

        public const string Demo = "demo";

        public static List<string> GetRoles()
        {
            return new List<string>
            {
                Admin,
                Demo
            };
        }
    }
}
