using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Security.Extensions
{
    public static class ExtIPrincipal
    {
        public static IEnumerable<string> GetRoles(this IPrincipal principal)
        {
            var userIdentity = principal.Identity as ClaimsIdentity;
            
            if (userIdentity == null || userIdentity.Claims == null)
                return new List<string>();
            
            return userIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(x=>x.Value);
        }
    }
}
