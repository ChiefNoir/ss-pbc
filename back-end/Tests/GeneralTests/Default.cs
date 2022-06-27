using Abstractions.Models;
using Abstractions.Security;
using Infrastructure.Migrations;

namespace GeneralTests
{
    internal static class Default
    {
        internal static readonly Introduction Introduction = new()
        {
            Title = M197201010000_InitializeDefaults.introductionTitle,
            Content = M197201010000_InitializeDefaults.introductionContent,
            ExternalUrls = new List<ExternalUrl>(),
            Version = 0,
            PosterUrl = string.Empty,
            PosterDescription = string.Empty
        };

        internal static readonly Account Account = new()
        {
            Login = "sa",
            Password = "sa",
            Role = RoleNames.Admin,
            Version = 0
        };

        internal static readonly Category Category = new()
        {
            Id = M197201010000_InitializeDefaults.categoryId,
            Code = M197201010000_InitializeDefaults.categoryCode,
            DisplayName = M197201010000_InitializeDefaults.categoryDisplayName,
            IsEverything = true,
            TotalProjects = 0,
            Version = 0,
        };
    }
}
