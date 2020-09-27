using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Security.Extensions
{
    public static class ExtIPrincipal
    {
        /// <summary> Get roles names from <seealso cref="IPrincipal"/> </summary>
        /// <param name="principal"> <seealso cref="IPrincipal"/> </param>
        /// <returns><seealso cref="IEnumerable"/> with roles names</returns>
        public static IEnumerable<string> GetRoles(this IPrincipal principal)
        {
            if(principal == null)
                return new List<string>();

            if(!(principal.Identity is ClaimsIdentity userIdentity))
                return new List<string>();

            if(userIdentity.Claims == null || !userIdentity.Claims.Any())
                return new List<string>();

            return userIdentity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
        }
    }
}
