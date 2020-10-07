using GeneralTests.SharedMocks;
using Security.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace GeneralTests.Security.Extensions
{
    public class ExtIPrincipalTests
    {
        private class ValidClaimsIdentity : IEnumerable<object[]>
        {
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new MockPrincipal(new ClaimsIdentity
                    (
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Role, "Role 1"),
                            new Claim(ClaimTypes.Role, "Role 2")
                        }
                    )),
                    new []{ "Role 1" , "Role 2" }
                };
                yield return new object[]
                {
                    new MockPrincipal(new ClaimsIdentity
                    (
                        new List<Claim>
                        {
                            new Claim(ClaimTypes.Actor, "Actor"),
                            new Claim(ClaimTypes.Role, "Role 1")
                        }
                    )),
                    new []{ "Role 1" }
                };
                yield return new object[]
                {
                    new MockPrincipal(new ClaimsIdentity
                    (
                        new List<Claim>()
                    )),
                    Array.Empty<string>()
                };
                yield return new object[]
                {
                    new MockPrincipal(new ClaimsIdentity()),
                    Array.Empty<string>()
                };
                yield return new object[]
                {
                    new MockPrincipal(null),
                    Array.Empty<string>()
                };
                yield return new object[]
                {
                    null,
                    Array.Empty<string>()
                };
            }
        }

        [Theory]
        [ClassData(typeof(ValidClaimsIdentity))]
        public void ExtIPrincipal_GetRoles(MockPrincipal principal, string[] expected)
        {
            var result = principal.GetRoles().OrderBy(x => x).ToArray();

            Assert.True(result.SequenceEqual(expected));
        }
    }
}
