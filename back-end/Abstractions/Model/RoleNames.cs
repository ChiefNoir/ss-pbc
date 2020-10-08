using System.Diagnostics.CodeAnalysis;

namespace Abstractions.Model
{
    [ExcludeFromCodeCoverage]
    public static class RoleNames
    {
        public static string Admin { get; } = "admin";

        public static string Demo { get; } = "demo";
    }
}