using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace GeneralTests.SharedMocks
{
    public class MockPrincipal : IPrincipal
    {
        public IIdentity Identity { get; }
        private readonly string _role;


        public MockPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            var claimsIdentity = Identity as ClaimsIdentity;

            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
                return false;

            foreach (var item in claimsIdentity.Claims)
            {
                if (item.Value == role)
                    return true;
            }

            return false;
        }
    }
}
