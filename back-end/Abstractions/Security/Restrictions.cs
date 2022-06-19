using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.Security
{
    public static class Restrictions
    {
        public const string AuthorizedRoles = RoleNames.Admin + " , " + RoleNames.Demo;

        public const string EditorRoles = RoleNames.Admin;
    }
}
