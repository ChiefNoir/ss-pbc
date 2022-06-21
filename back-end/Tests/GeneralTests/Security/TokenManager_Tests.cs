using Security;

namespace GeneralTests.Security
{
    [Trait("Category", "Unit")]
    public sealed class TokenManager_Tests
    {
        private readonly TokenManager _tokenManager;

        public TokenManager_Tests()
        {
            _tokenManager = new TokenManager(Initializer.CreateConfiguration());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12")]
        [InlineData("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic2EiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsIm5iZiI6MTYwMTI0OTAyMiwiZXhwIjoxNjAxMjUwODIyLCJpc3MiOiJJc3N1ZXJOYW1lIiwiYXVkIjoiQXVkaWVuY2UtMSJ9.Hjn6E29y1KokbN4_zN8STKIj6IhmYcmsRV-IeyoV39U")]
        public void ValidateToken_Invalid(string token)
        {
            Assert.ThrowsAny<Exception>(() => _tokenManager.ValidateToken(token));
        }

        [Theory]
        [InlineData("login", null)]
        [InlineData(null, new[] { "admin" })]
        [InlineData("login", new[] { "" })]
        [InlineData("login", new[] { "", "", "" })]
        [InlineData("login", new[] { null, null, "" })]
        [InlineData(null, new[] { "" })]
        public void CreateToken_Invalid(string login, string[] roles)
        {
            Assert.ThrowsAny<Exception>(() => _tokenManager.CreateToken(login, roles));
        }

        [Theory]
        [InlineData("login", new[] { "admin" })]
        [InlineData("root", new[] { "sa", "as" })]
        [InlineData("original", new[] { "role 1", null })]
        [InlineData("qwerty", new[] { "role 1", null, "" })]
        public void ValidateToken_Valid(string login, string[] roles)
        {
            var token = _tokenManager.CreateToken(login, roles);
            var principal = _tokenManager.ValidateToken(token);

            Assert.True(principal.Identity.Name == login);

            foreach (var item in roles.Where(x => !string.IsNullOrEmpty(x)))
            {
                Assert.True(principal.IsInRole(item));
            }
        }

    }
}
